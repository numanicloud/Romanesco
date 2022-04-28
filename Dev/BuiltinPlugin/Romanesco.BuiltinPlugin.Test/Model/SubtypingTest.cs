using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Moq;
using Romanesco.Annotations;
using Romanesco.BuiltinPlugin.Model.Basics;
using Romanesco.BuiltinPlugin.Model.Factories;
using Romanesco.BuiltinPlugin.Model.Infrastructure;
using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Model.Infrastructure;
using Xunit;

namespace Romanesco.BuiltinPlugin.Test.Model
{
	public class SubtypingTest
	{
		private class TestClass1
		{
			[EditorMember] public List<TestBase>? TestBase { get; set; }
		}

		[EditorSubtypingBase]
		private abstract class TestBase
		{
		}

		[EditorSubtypeName(nameof(TestDerived))]
		private class TestDerived : TestBase
		{
		}

		[Fact]
		public void List以下のSubtypingの中身が更新されると元のインスタンスに反映される()
		{
			var api = new Mock<IApiFactory>();
			api.Setup(m => m.OnProjectChanged).Returns(Observable.Never<Unit>());

			var interpreter = new ObjectInterpreter(new IStateFactory[]
			{
				new ListStateFactory(new MasterListContext(api.Object), new CommandHistory()),
				new SubtypingStateFactory(api.Object),
				new ClassStateFactory(new DataAssemblyRepository())
			});

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

			var element = new TestDerived();
			if (testBaseState is ListState list)
			{
				list.AddNewElement();
				// SubtypingClassStateの要素がひとつ追加されていること
				Assert.True(list.Elements[0] is SubtypingClassState);

				if (list.Elements[0] is SubtypingClassState scs)
				{
					// その要素はサブタイプコンテキストとしてNullSubtypeOptionが選択されていること
					Assert.IsType<NullSubtypeOption>(scs.SelectedType.Value);

					scs.SelectedType.Value = new ConcreteSubtypeOption(typeof(TestDerived),
						new ValueStorage(typeof(TestDerived),
							"0",
							(value, oldValue) => instance.TestBase![0] = (TestDerived)value!,
							null),
						new SubtypingStateContext(new SubtypingList(typeof(TestBase)),
							new DataAssemblyRepository(),
							interpreter));

					// 値の型がTestDerivedに変更されていること
					Assert.IsType<TestDerived>(scs.Storage.GetValue());
					Assert.IsType<TestDerived>(instance.TestBase![0]);
				}
			}
			else
			{
				Assert.True(false);
			}
		}
	}
}
