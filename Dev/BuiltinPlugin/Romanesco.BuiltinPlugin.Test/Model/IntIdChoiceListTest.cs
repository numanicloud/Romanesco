using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Reactive.Bindings;
using Romanesco.BuiltinPlugin.Model.Infrastructure;
using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.Model.ProjectComponent;
using Xunit;

namespace Romanesco.BuiltinPlugin.Test.Model
{
	public class IntIdChoiceListTest
	{
		[Fact]
		public async void 初期化時の要素数がValueStorageから読み込まれる()
		{
			// これを外すとエラーになる場合がある。
			// ToReadOnlyReactiveCollection が別スレッドで動いている可能性がある
			ReactivePropertyScheduler.SetDefault(ImmediateScheduler.Instance);

			var api = new Mock<IApiFactory>();
			api.Setup(m => m.OnProjectChanged).Returns(Observable.Never<Unit>());
			var master = new MasterListContext(api.Object);

			var list = new List<int>() { 1 };
			var storage = new ValueStorage(typeof(List<int>), "Fugas",
				(value, oldValue) => list = (List<int>)value!,
				list);
			var state = new IntIdChoiceListState(storage, "Hoges", master, new CommandHistory());

			Assert.Single(state.Elements);
			Assert.Single(((List<int>)state.Storage.GetValue()!));
		}

		[Fact]
		public void 新しい要素を追加することができる()
		{
			ReactivePropertyScheduler.SetDefault(ImmediateScheduler.Instance);

			var api = new Mock<IApiFactory>();
			api.Setup(m => m.OnProjectChanged).Returns(Observable.Never<Unit>());
			var master = new MasterListContext(api.Object);

			var list = new List<int>();
			var storage = new ValueStorage(typeof(List<int>), "Fugas",
				(value, oldValue) => list = (List<int>)value!,
				list);
			var state = new IntIdChoiceListState(storage, "Hoges", master, new CommandHistory());

			state.AddNewElement();

			Assert.Single(state.Elements);
			Assert.Equal(0, state.Elements[0].Storage.GetValue());
			Assert.Single((List<int>)state.Storage.GetValue()!);
			Assert.Equal(0, ((List<int>)state.Storage.GetValue()!)[0]);
		}
	}
}
