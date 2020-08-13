using Moq;
using Romanesco.ViewModel.Editor;
using System;
using System.Collections.Generic;
using System.Threading;
using Reactive.Bindings;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.Commands;
using Romanesco.Model.EditorComponents;
using Romanesco.Model.EditorComponents.States;
using Romanesco.Model.Interfaces;
using Romanesco.Model.Services.Load;
using Romanesco.Model.Services.Save;
using Romanesco.Test.Helpers;
using Romanesco.ViewModel.States;
using Romanesco.ViewModel.Test.Helpers;
using Xunit;
using static Romanesco.Model.EditorComponents.EditorCommandType;

namespace Romanesco.ViewModel.Test.Editor
{
	public class EditorViewModelTest
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

			var model = new Mock<IEditorFacade>();
			model.Setup(x => x.CommandAvailabilityPublisher)
				.Returns(commands);

			var disposables = new List<IDisposable>();
			model.Setup(x => x.Disposables)
				.Returns(disposables);

			var editor = new EditorViewModel(model.Object, Mock.Of<IViewModelInterpreter>());

			var targetCommand = type switch
			{
				Create => editor.CreateCommand,
				Open => editor.OpenCommand,
				Save => editor.SaveCommand,
				SaveAs => editor.SaveAsCommand,
				Export => editor.ExportCommand,
				Undo => editor.Undo,
				Redo => editor.Redo,
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

			var model = GetEditorModel();
			model.Setup(x => x.CommandAvailabilityPublisher)
				.Returns(commands.Object);

			var viewModel = new EditorViewModel(model.Object, Mock.Of<IViewModelInterpreter>());

			viewModel.CreateCommand.Execute(null);

			commands.Verify(x => x.CreateAsync(), Times.Once);
		}

		[Fact]
		public void Openコマンドがモデルに伝わる()
		{
			var commands = GetCommands();
			commands.Setup(x => x.OpenAsync())
				.Callback(async () => { });

			var model = GetEditorModel();
			model.Setup(x => x.CommandAvailabilityPublisher)
				.Returns(commands.Object);

			var viewModel = new EditorViewModel(model.Object, Mock.Of<IViewModelInterpreter>());

			viewModel.OpenCommand.Execute(null);

			commands.Verify(x => x.OpenAsync(), Times.Once);
		}

		[Fact]
		public void SaveAsコマンドがモデルに伝わる()
		{
			var commands = GetCommands();
			commands.Setup(x => x.SaveAsAsync())
				.Callback(async () => { });

			var model = GetEditorModel();
			model.Setup(x => x.CommandAvailabilityPublisher)
				.Returns(commands.Object);

			var viewModel = new EditorViewModel(model.Object, Mock.Of<IViewModelInterpreter>());

			viewModel.SaveAsCommand.Execute(null);

			commands.Verify(x => x.SaveAsAsync(), Times.Once);
		}

		private static Mock<IEditorFacade> GetEditorModel()
		{
			var disposables = new List<IDisposable>();
			var model = new Mock<IEditorFacade>();
			model.Setup(x => x.Disposables)
				.Returns(disposables);
			return model;
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

		[Fact]
		public void プロジェクトを作成した際に新しいステートへのメッセージが届く()
		{
			static Mock<IEditorState> GetMockEditorState(string name, IProjectLoadService load)
			{
				var mock = new Mock<IEditorState>() { Name = name };
				mock.Setup(x => x.GetLoadService()).Returns(load);
				mock.Setup(x => x.GetSaveService())
					.Returns(MockHelper.GetSaveServiceMock(false, false).Object);
				mock.Setup(x => x.GetHistoryService())
					.Returns(MockHelper.CreateHistoryMock(canUndo: false, canRedo: false).Object);
				mock.Setup(x => x.OnCreate(It.IsAny<IProjectContext>()))
					.Callback(() => { });
				return mock;
			}

			var project = new Mock<IProjectContext>();
			project.Setup(x => x.Project)
				.Returns(() =>
				{
					var p = new Mock<IProject>();
					p.Setup(x => x.Root)
						.Returns(() => new StateRoot(new object(), new IFieldState[0]));
					return p.Object;
				});

			var loadService = new Mock<IProjectLoadService>();
			loadService.Setup(x => x.CreateAsync())
				.Returns(async () => project.Object);

			var currentState = GetMockEditorState("Current", loadService.Object);
			var nextState = GetMockEditorState("Next", loadService.Object);
			var commandRouter = new CommandRouter(currentState.Object);
			commandRouter.OnCreate.Subscribe(x => commandRouter.UpdateState(nextState.Object));

			var model = GetEditorModel();

			model.Setup(x => x.CommandAvailabilityPublisher)
				.Returns(() => commandRouter);

			var editor = new EditorViewModel(model.Object, Mock.Of<IViewModelInterpreter>());
			editor.CreateCommand.Execute(null);
			editor.CreateCommand.Execute(null);

			nextState.Verify(x => x.OnCreate(It.IsAny<IProjectContext>()), Times.Once);
			currentState.Verify(x => x.OnCreate(It.IsAny<IProjectContext>()), Times.Once);
		}
	}
}
