using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Moq;
using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.Commands;
using Romanesco.Model.EditorComponents;
using Romanesco.Model.EditorComponents.States;
using Romanesco.Model.Infrastructure;
using Romanesco.Model.Services.History;
using Romanesco.Model.Services.Load;
using Romanesco.Model.Services.Save;
using Xunit;

namespace Romanesco.Test.EditorComponents
{
	public class EditorStateTest
	{
		[Fact]
		public void Undoを呼ぶとUndo可能性が更新される()
		{
			var availability = new CommandAvailability();
			var editorState = GetDirtyEditorStateToHistory(CreateHistoryMock(), availability);

			bool raised = false;
			availability.Observable.Where(x => x.Item1 == EditorCommandType.Undo)
				.Subscribe(x => raised = true);

			editorState.Undo(availability);

			Assert.True(raised);
		}
		
		[Fact]
		public void Redoを呼ぶとRedo可能性が更新される()
		{
			var availability = new CommandAvailability();
			var editorState = GetDirtyEditorStateToHistory(CreateHistoryMock(), availability);

			bool raised = false;
			availability.Observable.Where(x => x.Item1 == EditorCommandType.Redo)
				.Subscribe(x => raised = true);

			editorState.Redo(availability);

			Assert.True(raised);
		}
		
		[Theory]
		[InlineData(EditorCommandType.Undo)]
		[InlineData(EditorCommandType.Redo)]
		public void UndoとRedoの状態を更新できる(EditorCommandType type)
		{
			// これはもはや CommandAvailability のテスト
			var availability = new CommandAvailability();
			var editorState = GetDirtyEditorStateToHistory(CreateHistoryMock(), availability);

			bool raised = false;
			availability.Observable.Where(x => x.Item1 == type)
				.Subscribe(x => raised = true);

			availability.UpdateCanExecute(editorState.GetHistoryService());

			Assert.True(raised);
		}

		private static Mock<IProjectHistoryService> CreateHistoryMock()
		{
			var history = new Mock<IProjectHistoryService>();
			history.Setup(x => x.Undo())
				.Callback(() => { });
			history.Setup(x => x.CanUndo).Returns(true);
			history.Setup(x => x.Redo())
				.Callback(() => { });
			history.Setup(x => x.CanRedo).Returns(true);
			return history;
		}

		private static DirtyEditorState GetDirtyEditorStateToHistory(Mock<IProjectHistoryService> history,
			CommandAvailability commandAvailability)
		{
			var editorState = new DirtyEditorState(
				Mock.Of<IProjectLoadService>(),
				history.Object,
				Mock.Of<IProjectSaveService>(),
				Mock.Of<IProjectContext>(),
				Mock.Of<IProjectModelFactory>(),
				Mock.Of<IEditorStateChanger>(),
				commandAvailability);
			return editorState;
		}
		
		[Fact]
		public void 与えたIProjectSaveServiceでプロジェクトを保存できる()
		{
			var saveService = GetMockSaveService();
			var editorState = GetMockEditorStateToSave(saveService, new CommandAvailability());

			editorState.SaveAsync().Wait();

			saveService.Verify(x => x.SaveAsync(), Times.Once);
		}

		[Fact]
		public void 与えたIProjectSaveServiceでプロジェクトを上書き保存できる()
		{
			var saveService = GetMockSaveService();
			var editorState = GetMockEditorStateToSave(saveService, new CommandAvailability());

			editorState.SaveAsAsync().Wait();

			saveService.Verify(x => x.SaveAsAsync(), Times.Once);
		}

		[Fact]
		public void 与えたIProjectSaveServiceでプロジェクトをエクスポートできる()
		{
			var saveService = GetMockSaveService();
			var editorState = GetMockEditorStateToSave(saveService, new CommandAvailability());

			editorState.ExportAsync().Wait();

			saveService.Verify(x => x.ExportAsync(), Times.Once);
		}

		private static Mock<IProjectSaveService> GetMockSaveService()
		{
			var saveService = new Mock<IProjectSaveService>();
			saveService.Setup(x => x.SaveAsync())
				.Callback(async () => { });
			saveService.Setup(x => x.SaveAsAsync())
				.Callback(async () => { });
			saveService.Setup(x => x.ExportAsync())
				.Callback(async () => { });
			return saveService;
		}

		private static DirtyEditorState GetMockEditorStateToSave(Mock<IProjectSaveService> saveService,
			CommandAvailability commandAvailability)
		{
			return new DirtyEditorState(
				Mock.Of<IProjectLoadService>(),
				Mock.Of<IProjectHistoryService>(),
				saveService.Object,
				Mock.Of<IProjectContext>(),
				Mock.Of<IProjectModelFactory>(),
				Mock.Of<IEditorStateChanger>(),
				commandAvailability);
		}

		[Fact]
		public void OnEditを呼ぶとUndoが更新される()
		{
			var history = CreateHistoryMock();
			var commandAvailability = new CommandAvailability();
			var editorState = GetDirtyEditorStateToHistory(history, commandAvailability);

			bool raised = false;
			commandAvailability.Observable
				.Where(x => x.command == EditorCommandType.Undo)
				.Subscribe(x => raised = true);

			editorState.NotifyEdit(commandAvailability);

			Assert.True(raised);
		}

		[Fact]
		public void OnEditを呼ぶとRedoが更新される()
		{
			var history = CreateHistoryMock();
			var commandAvailability = new CommandAvailability();
			var editorState = GetDirtyEditorStateToHistory(history, commandAvailability);

			bool raised = false;
			commandAvailability.Observable
				.Where(x => x.command == EditorCommandType.Redo)
				.Subscribe(x => raised = true);

			editorState.NotifyEdit(commandAvailability);

			Assert.True(raised);
		}

		[Fact]
		public void プロジェクトを作成するサービスを実行できる()
		{
			var loadService = new Mock<IProjectLoadService>();
			loadService.Setup(x => x.CreateAsync())
				.Returns(async () => null);

			var editorState = new DirtyEditorState(
				loadService.Object,
				Mock.Of<IProjectHistoryService>(),
				Mock.Of<IProjectSaveService>(),
				Mock.Of<IProjectContext>(),
				Mock.Of<IProjectModelFactory>(),
				Mock.Of<IEditorStateChanger>(),
				new CommandAvailability());

			var _ = editorState.CreateAsync().Result;

			loadService.Verify(x => x.CreateAsync(), Times.Once);
		}
		
		[Fact]
		public void プロジェクトを開くサービスを実行できる()
		{
			var loadService = new Mock<IProjectLoadService>();
			loadService.Setup(x => x.OpenAsync())
				.Returns(async () => null);

			var editorState = new DirtyEditorState(
				loadService.Object,
				Mock.Of<IProjectHistoryService>(),
				Mock.Of<IProjectSaveService>(),
				Mock.Of<IProjectContext>(),
				Mock.Of<IProjectModelFactory>(),
				Mock.Of<IEditorStateChanger>(),
				new CommandAvailability());

			var _ = editorState.OpenAsync().Result;

			loadService.Verify(x => x.OpenAsync(), Times.Once);
		}
	}
}
