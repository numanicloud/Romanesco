using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Moq;
using Romanesco.Annotations;
using Romanesco.BuiltinPlugin.Model.Basics;
using Romanesco.BuiltinPlugin.Model.Factories;
using Romanesco.BuiltinPlugin.Model.Infrastructure;
using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Implementations;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.Model.Reflections;
using Romanesco.Model.Infrastructure;
using Romanesco.Model.Services;
using Romanesco.Model.Services.Serialize;
using Xunit;

namespace Romanesco.BuiltinPlugin.Test.Model
{
	public class SubtypingTest
	{
		private class TestClass1
		{
			[EditorMember] public List<TestBase>? TestBase { get; set; }
		}

		private class TestClass2
		{
			[EditorMember] public TestBase? Subtyping { get; set; }
		}

		private class TestClass3
		{
			[EditorMember] public TestClass2 Item { get; set; } = new TestClass2();
		}

		[EditorSubtypingBase]
		public abstract class TestBase
		{
		}

		[EditorSubtypeName(nameof(TestDerived))]
		public class TestDerived : TestBase
		{
		}

		[EditorSubtypeName(nameof(TestDerived2))]
		public class TestDerived2 : TestBase
		{
			[EditorMember]
			public int Hoge { get; set; }
		}

		private class MockStore
		{
			public Mock<IApiFactory> ApiFactory { get; } = new();
			public Mock<IDataAssemblyRepository> DataAssemblyRepository { get; } = new();
			public MockObjectInterpreter ObjectInterpreter { get; }

			public MockStore()
			{
				ApiFactory.Setup(m => m.OnProjectChanged).Returns(Observable.Never<Unit>());
				ApiFactory.Setup(m => m.ResolveValueClipBoard()).Returns(() => new ValueClipBoard());
				ApiFactory.Setup(m => m.ResolveDataAssemblyRepository())
					.Returns(() => DataAssemblyRepository.Object);

				DataAssemblyRepository.Setup(m => m.CreateInstance(It.IsAny<Type>(), It.IsAny<object[]>()))
					.Returns<Type, object[]>((type, objects) => Activator.CreateInstance(type, objects));

				ObjectInterpreter = new MockObjectInterpreter(this);
			}
		}

		private class MockObjectInterpreter : IObjectInterpreter
		{
			public ClassStateFactory ClassStateFactory { get; }
			private readonly ListStateFactory _listStateFactory;
			private readonly SubtypingStateFactory _subtypingStateFactory;
			private readonly PrimitiveStateFactory _primitiveStateFactory;

			public MockObjectInterpreter(MockStore mockStore)
			{
				var api = mockStore.ApiFactory.Object;
				var commandHistory = new CommandHistory();

				ClassStateFactory =
					new ClassStateFactory(api.ResolveDataAssemblyRepository(), api);
				_listStateFactory =
					new ListStateFactory(new MasterListContext(api), commandHistory);
				_subtypingStateFactory = new SubtypingStateFactory(api, ClassStateFactory);
				_primitiveStateFactory = new PrimitiveStateFactory(commandHistory);
			}

			public IFieldState InterpretAsState(ValueStorage storage)
			{
				return _listStateFactory.InterpretAsState(storage, InterpretAsState)
					?? _subtypingStateFactory.InterpretAsState(storage, InterpretAsState)
					?? ClassStateFactory.InterpretAsState(storage, InterpretAsState)
					?? _primitiveStateFactory.InterpretAsState(storage, InterpretAsState)
					?? new NoneState();
			}
		}

		[Fact]
		public void List以下のSubtypingの中身が更新されると元のインスタンスに反映される()
		{
			var mocks = new MockStore();
			var api = mocks.ApiFactory.Object;
			var interpreter = mocks.ObjectInterpreter;

			// インスタンスとStateを構築する
			var instance = new TestClass1();
			var testBaseValue = new ValueStorage(typeof(List<TestBase>),
				"TestBase",
				(value, oldValue) => instance.TestBase = (List<TestBase>?)value,
				null);
			var testBaseState = interpreter.InterpretAsState(testBaseValue);
			var listInstance = testBaseState.Storage.GetValue();
			
			// インスタンスのTestBaseと、testBaseStateのStorageの参照が等しい
			Assert.Equal(instance.TestBase, listInstance);
			AssertTypeOf<ListState>(testBaseState, out var list);
			
			list.AddNewElement();
			// SubtypingClassStateの要素がひとつ追加されていること
			Assert.True(list.Elements[0] is SubtypingClassState);
			AssertTypeOf<SubtypingClassState>(list.Elements[0], out var scs);
			
			// その要素はサブタイプコンテキストとしてNullSubtypeOptionが選択されていること
			Assert.IsType<NullSubtypeOption>(scs.SelectedType.Value);

			scs.SelectedType.Value = new ConcreteSubtypeOption(typeof(TestDerived),
				new SubtypingStateContext(new SubtypingList(typeof(TestBase)),
					api.ResolveDataAssemblyRepository(),
					interpreter),
				interpreter.ClassStateFactory,
				api.ResolveStorageCloneService());

			// 値の型がTestDerivedに変更されていること
			Assert.IsType<TestDerived>(scs.Storage.GetValue());
			Assert.IsType<TestDerived>(instance.TestBase![0]);
		}

		[Fact]
		public void PasteしたあとにValueStorageの親がクローンされていること()
		{
			var api = new Mock<IApiFactory>();
			var clipboard = new ValueClipBoard();
			var asmRepo = new DataAssemblyRepository();
			var classFactory = new ClassStateFactory(asmRepo, new Mock<ILoadingStateReader>().Object);
			var interpreter = new ObjectInterpreter(new IStateFactory[]
			{
				new SubtypingStateFactory(api.Object, classFactory),
				classFactory,
				new PrimitiveStateFactory(new CommandHistory()),
			});

			api.Setup(m => m.OnProjectChanged).Returns(Observable.Never<Unit>());
			api.Setup(m => m.ResolveValueClipBoard()).Returns(() => clipboard);
			api.Setup(m => m.ResolveDataAssemblyRepository()).Returns(() => asmRepo);
			api.Setup(m => m.ResolveObjectInterpreter()).Returns(() => interpreter);
			api.Setup(m => m.ResolveStorageCloneService())
				.Returns(() => new StorageCloneService(new NewtonsoftStateSerializer(),
					new NewtonsoftStateDeserializer()));
;
			var parent1 = new TestClass3();
			var parent2 = new TestClass3();
			var storage = new ValueStorage(parent1, typeof(TestClass3).GetProperty("Item")!);
			var storage2 = new ValueStorage(parent2, typeof(TestClass3).GetProperty("Item")!);
			var state = interpreter.InterpretAsState(storage);
			var state2 = interpreter.InterpretAsState(storage2);

			AssertTypeOf<ClassState>(state, out var parentState1);
			AssertTypeOf<ClassState>(state2, out var parentState2);
			AssertTypeOf<SubtypingClassState>(parentState1.Fields[0], out var subtyping);
			AssertTypeOf<SubtypingClassState>(parentState2.Fields[0], out var subtyping2);

			subtyping.SelectedType.Value = subtyping.Choices.First(x => x.IsTypeOf(typeof(TestDerived2)));

			subtyping.Copy();
			subtyping2.Paste();

			AssertTypeOf<ClassState>(subtyping.CurrentStateReadOnly.Value, out var class1);
			AssertTypeOf<ClassState>(subtyping2.CurrentStateReadOnly.Value, out var class2);

			var s1 = class1.Fields[0].Storage;
			var s2= class2.Fields[0].Storage;

			class1.Fields[0].Storage.SetValue(1);
			class2.Fields[0].Storage.SetValue(2);

			var prop = typeof(TestDerived2).GetProperty("Hoge");
			Assert.Equal(1, prop!.GetValue(class1.Storage.GetValue()));
			Assert.Equal(2, prop!.GetValue(class2.Storage.GetValue()));
		}

		private void AssertTypeOf<T>(object instance, out T result)
		{
			Assert.True(instance.GetType().IsAssignableTo(typeof(T)));
			result = (T)instance;
		}
	}
}
