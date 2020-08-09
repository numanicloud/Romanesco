using System;
using System.Threading;
using Moq;
using Reactive.Bindings;
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
	public class CommandManagerViewModelTest
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

			var commands = new CommandAvailability(Mock.Of<IEditorState>());
			var subject = new CommandManagerViewModel(
				commands,
				new ReactiveProperty<IStateViewModel[]>(),
				Mock.Of<IViewModelInterpreter>());

			var targetCommand = type switch
			{
				Create => subject.Create,
				Open => subject.Open,
				Save => subject.Save,
				SaveAs => subject.SaveAs,
				Export => subject.Export,
				Undo => subject.Undo,
				Redo => subject.Redo,
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
			var commands = GetCommands();
			commands.Setup(x => x.CreateAsync())
				.Callback(async () => { });

			var subject = new CommandManagerViewModel(
				commands.Object,
				new ReactiveProperty<IStateViewModel[]>(),
				Mock.Of<IViewModelInterpreter>());

			subject.Create.Execute(null);

			commands.Verify(x => x.CreateAsync(), Times.Once);
		}
		
		[Fact]
		public void Openコマンドがモデルに伝わる()
		{
			var commands = GetCommands();
			commands.Setup(x => x.OpenAsync())
				.Callback(async () => { });

			var subject = new CommandManagerViewModel(
				commands.Object,
				new ReactiveProperty<IStateViewModel[]>(),
				Mock.Of<IViewModelInterpreter>());

			subject.Open.Execute(null);

			commands.Verify(x => x.OpenAsync(), Times.Once);
		}
		
		[Fact]
		public void Saveコマンドがモデルに伝わる()
		{
			var commands = GetCommands();
			commands.Setup(x => x.SaveAsync())
				.Callback(async () => { });

			var subject = new CommandManagerViewModel(
				commands.Object,
				new ReactiveProperty<IStateViewModel[]>(),
				Mock.Of<IViewModelInterpreter>());

			subject.Save.Execute();

			commands.Verify(x => x.SaveAsync(), Times.Once);
		}
		
		[Fact]
		public void SaveAsコマンドがモデルに伝わる()
		{
			var commands = GetCommands();
			commands.Setup(x => x.SaveAsAsync())
				.Callback(async () => { });

			var subject = new CommandManagerViewModel(
				commands.Object,
				new ReactiveProperty<IStateViewModel[]>(),
				Mock.Of<IViewModelInterpreter>());

			subject.SaveAs.Execute();

			commands.Verify(x => x.SaveAsAsync(), Times.Once);
		}
		
		[Fact]
		public void Exportコマンドがモデルに伝わる()
		{
			var commands = GetCommands();
			commands.Setup(x => x.ExportAsync())
				.Callback(async () => { });

			var subject = new CommandManagerViewModel(
				commands.Object,
				new ReactiveProperty<IStateViewModel[]>(),
				Mock.Of<IViewModelInterpreter>());

			subject.Export.Execute();

			commands.Verify(x => x.ExportAsync(), Times.Once);
		}
		
		[Fact]
		public void Undoコマンドがモデルに伝わる()
		{
			var commands = GetCommands();
			commands.Setup(x => x.Undo())
				.Callback(async () => { });

			var subject = new CommandManagerViewModel(
				commands.Object,
				new ReactiveProperty<IStateViewModel[]>(),
				Mock.Of<IViewModelInterpreter>());

			subject.Undo.Execute();

			commands.Verify(x => x.Undo(), Times.Once);
		}
		
		[Fact]
		public void Redoコマンドがモデルに伝わる()
		{
			var commands = GetCommands();
			commands.Setup(x => x.Redo())
				.Callback(async () => { });

			var subject = new CommandManagerViewModel(
				commands.Object,
				new ReactiveProperty<IStateViewModel[]>(),
				Mock.Of<IViewModelInterpreter>());

			subject.Redo.Execute();

			commands.Verify(x => x.Redo(), Times.Once);
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
