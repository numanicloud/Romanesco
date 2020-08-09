using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Moq;
using Reactive.Bindings;
using Romanesco.Common.ViewModel.Interfaces;
using Romanesco.Model.Commands;
using Romanesco.Model.EditorComponents;
using Romanesco.Model.EditorComponents.States;
using Romanesco.Model.Interfaces;
using Romanesco.ViewModel.Commands;
using Romanesco.ViewModel.Editor;
using Romanesco.ViewModel.States;
using Romanesco.ViewModel.Test.Helpers;
using Xunit;
using static Romanesco.Model.EditorComponents.EditorCommandType;

namespace Romanesco.ViewModel.Test.Commands
{
	public class CommandManagerViewModelTest
	{
		[Theory]
		[InlineData(Create)]
		public void コマンドの実行可能性が反映される(EditorCommandType type)
		{
			SynchronizationContext.SetSynchronizationContext(new TestSynchronizationContext());

			var commands = new CommandAvailability(Mock.Of<IEditorState>());
			var subject = new CommandManagerViewModel(
				commands,
				new ReactiveProperty<IStateViewModel[]>(),
				Mock.Of<IViewModelInterpreter>());

			var targetCommand = type switch
			{
				Create => subject.Create,
				_ => throw new NotImplementedException(),
			};

			commands.UpdateCanExecute(type, false);
			Assert.False(targetCommand.CanExecute());

			commands.UpdateCanExecute(type, true);
			Assert.True(targetCommand.CanExecute());
		}

		[Fact]
		public void Createコマンドがモデルに伝わる()
		{
			var commands = GetCommands();
			commands.Setup(x => x.CreateAsync())
				.Callback(async () => { });

			var subject = new CommandManagerViewModel(
				commands.Object,
				new ReactiveProperty<IStateViewModel[]>(),
				Mock.Of<IViewModelInterpreter>());

			subject.Create.Execute();

			commands.Verify(x => x.CreateAsync(), Times.Once);
		}

		private static Mock<ICommandAvailabilityPublisher> GetCommands()
		{
			var commands = new Mock<ICommandAvailabilityPublisher>();
			commands.Setup(x => x.CanCreate).Returns(new ReactiveProperty<bool>(true));
			commands.Setup(x => x.CanOpen).Returns(new ReactiveProperty<bool>(true));
			commands.Setup(x => x.CanSave).Returns(new ReactiveProperty<bool>(true));
			commands.Setup(x => x.CanSaveAs).Returns(new ReactiveProperty<bool>(true));
			commands.Setup(x => x.CanExport).Returns(new ReactiveProperty<bool>(true));
			commands.Setup(x => x.CanUndo).Returns(new ReactiveProperty<bool>(true));
			commands.Setup(x => x.CanRedo).Returns(new ReactiveProperty<bool>(true));
			return commands;
		}
	}
}
