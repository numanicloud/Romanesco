using System;
using System.Linq.Expressions;
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
		[InlineData(SaveAs)]
		[InlineData(Export)]
		[InlineData(Undo)]
		[InlineData(Redo)]
		public void コマンドの実行可能性が反映される(EditorCommandType type)
		{
			SynchronizationContext.SetSynchronizationContext(new TestSynchronizationContext());

			var commands = new CommandAvailability(Mock.Of<IEditorState>(), Mock.Of<IEditorStateRepository>());
			var roots = new ReactiveProperty<IStateViewModel[]>();
			var interpreter = Mock.Of<IViewModelInterpreter>();
			var commandExecution = new BooleanUsingScopeSource();

			ICommand targetCommand = type switch
			{
				Create => new CreateCommandViewModel(commands, roots, interpreter, commandExecution),
				Open => new OpenCommandViewModel(commands, commandExecution, roots, interpreter),
				Save => new SaveCommandViewModel(commands, commandExecution),
				SaveAs => new SaveAsCommandViewModel(commands, commandExecution),
				Export => new ExportCommandViewModel(commands, commandExecution),
				Undo => new UndoCommandViewModel(commands, commandExecution),
				Redo => new RedoCommandViewModel(commands, commandExecution),
				_ => throw new NotImplementedException(),
			};

			commands.UpdateCanExecute(type, false);
			Assert.False(targetCommand.CanExecute(null));

			commands.UpdateCanExecute(type, true);
			Assert.True(targetCommand.CanExecute(null));
		}

		private void AssertCommandGoToModel<TCommandResult>(
			Expression<Func<ICommandAvailabilityPublisher, IReadOnlyReactiveProperty<bool>>> canExecuteExpression,
			Expression<Func<ICommandAvailabilityPublisher, TCommandResult>> executeExpression,
			Func<ICommandAvailabilityPublisher, ICommand> createCommand)
		{
			var model = new Mock<ICommandAvailabilityPublisher>();
			model.Setup(canExecuteExpression).Returns(new ReactiveProperty<bool>(true));
			model.Setup(executeExpression).Callback(async () => { });

			var command = createCommand(model.Object);

			command.Execute(null);

			model.Verify(executeExpression, Times.Once);
		}
		
		private void AssertCommandGoToModel(
			Expression<Func<ICommandAvailabilityPublisher, IReadOnlyReactiveProperty<bool>>> canExecuteExpression,
			Expression<Action<ICommandAvailabilityPublisher>> executeExpression,
			Func<ICommandAvailabilityPublisher, ICommand> createCommand)
		{
			var model = new Mock<ICommandAvailabilityPublisher>();
			model.Setup(canExecuteExpression).Returns(new ReactiveProperty<bool>(true));
			model.Setup(executeExpression).Callback(() => { });

			var command = createCommand(model.Object);

			command.Execute(null);

			model.Verify(executeExpression, Times.Once);
		}

		[Fact]
		public void Createコマンドがモデルに伝わる()
		{
			var roots = new ReactiveProperty<IStateViewModel[]>();
			var interpreter = Mock.Of<IViewModelInterpreter>();
			var commandExecution = new BooleanUsingScopeSource();

			AssertCommandGoToModel(x => x.CanCreate,
				x => x.CreateAsync(),
				p => new CreateCommandViewModel(p, roots, interpreter, commandExecution));
		}

		[Fact]
		public void Openコマンドがモデルに伝わる()
		{
			var roots = new ReactiveProperty<IStateViewModel[]>();
			var interpreter = Mock.Of<IViewModelInterpreter>();
			var commandExecution = new BooleanUsingScopeSource();
			
			AssertCommandGoToModel(x => x.CanOpen,
				x => x.OpenAsync(),
				p => new OpenCommandViewModel(p, commandExecution, roots, interpreter));
		}

		[Fact]
		public void Saveコマンドがモデルに伝わる()
		{
			var commandExecution = new BooleanUsingScopeSource();

			AssertCommandGoToModel(x => x.CanSave,
				x => x.SaveAsync(),
				p => new SaveCommandViewModel(p, commandExecution));
		}

		[Fact]
		public void SaveAsコマンドがモデルに伝わる()
		{
			var commandExecution = new BooleanUsingScopeSource();

			AssertCommandGoToModel(x => x.CanSaveAs,
				x => x.SaveAsAsync(),
				p => new SaveAsCommandViewModel(p, commandExecution));
		}

		[Fact]
		public void Exportコマンドがモデルに伝わる()
		{
			var commandExecution = new BooleanUsingScopeSource();

			AssertCommandGoToModel(x => x.CanExport,
				x => x.ExportAsync(),
				p => new ExportCommandViewModel(p, commandExecution));
		}

		[Fact]
		public void Undoコマンドがモデルに伝わる()
		{
			var commandExecution = new BooleanUsingScopeSource();

			AssertCommandGoToModel(x => x.CanUndo,
				x => x.Undo(),
				p => new UndoCommandViewModel(p, commandExecution));
		}

		[Fact]
		public void Redoコマンドがモデルに伝わる()
		{
			var commandExecution = new BooleanUsingScopeSource();

			AssertCommandGoToModel(x => x.CanRedo,
				x => x.Redo(),
				p => new RedoCommandViewModel(p, commandExecution));
		}
	}
}
