using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Input;
using Moq;
using Reactive.Bindings;
using Romanesco.Common.Model.Helpers;
using Romanesco.Common.ViewModel.Interfaces;
using Romanesco.Model.Commands;
using Romanesco.Model.EditorComponents;
using Romanesco.Model.EditorComponents.States;
using Romanesco.Model.Interfaces;
using Romanesco.ViewModel.Commands;
using Romanesco.ViewModel.States;
using Romanesco.ViewModel.Test.Helpers;
using Xunit;
using static Romanesco.Model.EditorComponents.EditorCommandType;

namespace Romanesco.ViewModel.Test.Commands
{
	public class CommandViewModelTest
	{
		[Theory]
		[InlineData(Create)]
		[InlineData(Open)]
		[InlineData(Save)]
		public void コマンドの実行可能性が反映される(EditorCommandType type)
		{
			SynchronizationContext.SetSynchronizationContext(new TestSynchronizationContext());

			var commands = new CommandAvailability(Mock.Of<IEditorState>());
			var roots = new ReactiveProperty<IStateViewModel[]>();
			var interpreter = Mock.Of<IViewModelInterpreter>();
			var commandExecution = new BooleanUsingScopeSource();

			ICommand targetCommand = type switch
			{
				Create => new CreateCommandViewModel(commands, roots, interpreter, commandExecution),
				Open => new OpenCommandViewModel(commands, commandExecution, roots, interpreter),
				Save => new SaveCommandViewModel(commands, commandExecution),
				_ => throw new NotImplementedException(),
			};

			commands.UpdateCanExecute(type, false);
			Assert.False(targetCommand.CanExecute(null));

			commands.UpdateCanExecute(type, true);
			Assert.True(targetCommand.CanExecute(null));
		}

		[Fact]
		public void Createコマンドがモデルに伝わる()
		{
			var commands = new Mock<ICommandAvailabilityPublisher>();
			commands.Setup(x => x.CanCreate)
				.Returns(new ReactiveProperty<bool>(true));
			commands.Setup(x => x.CreateAsync())
				.Callback(async () => { });
			
			var roots = new ReactiveProperty<IStateViewModel[]>();
			var interpreter = Mock.Of<IViewModelInterpreter>();
			var commandExecution = new BooleanUsingScopeSource();
			var subject = new CreateCommandViewModel(commands.Object, roots, interpreter, commandExecution);

			subject.Execute(null);

			commands.Verify(x => x.CreateAsync(), Times.Once);
		}

		[Fact]
		public void Openコマンドがモデルに伝わる()
		{
			var commands = new Mock<ICommandAvailabilityPublisher>();
			commands.Setup(x => x.CanOpen)
				.Returns(new ReactiveProperty<bool>(true));
			commands.Setup(x => x.OpenAsync())
				.Callback(async () => { });
			
			var roots = new ReactiveProperty<IStateViewModel[]>();
			var interpreter = Mock.Of<IViewModelInterpreter>();
			var commandExecution = new BooleanUsingScopeSource();
			var subject = new OpenCommandViewModel(commands.Object, commandExecution, roots, interpreter);

			subject.Execute(null);

			commands.Verify(x => x.OpenAsync(), Times.Once);
		}

		[Fact]
		public void Saveコマンドがモデルに伝わる()
		{
			var commands = new Mock<ICommandAvailabilityPublisher>();
			commands.Setup(x => x.CanSave)
				.Returns(new ReactiveProperty<bool>(true));
			commands.Setup(x => x.SaveAsync())
				.Callback(async () => { });
			
			var commandExecution = new BooleanUsingScopeSource();
			var subject = new SaveCommandViewModel(commands.Object, commandExecution);

			subject.Execute(null);

			commands.Verify(x => x.SaveAsync(), Times.Once);
		}
	}
}
