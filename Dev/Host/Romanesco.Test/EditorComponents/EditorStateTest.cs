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
			var editorState = GetDirtyEditorStateToHistory(CreateHistoryMock());
			var availability = new CommandAvailability();

			bool raised = false;
			availability.Observable.Where(x => x.Item1 == EditorCommandType.Undo)
				.Subscribe(x => raised = true);

			editorState.Undo(availability);

			Assert.True(raised);
		}
		
		[Fact]
		public void Redoを呼ぶとRedo可能性が更新される()
		{
			var editorState = GetDirtyEditorStateToHistory(CreateHistoryMock());
			var availability = new CommandAvailability();

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
			var editorState = GetDirtyEditorStateToHistory(CreateHistoryMock());
			var availability = new CommandAvailability();

			bool raised = false;
			availability.Observable.Where(x => x.Item1 == type)
				.Subscribe(x => raised = true);

			editorState.UpdateHistoryAvailability(availability);

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

		private static DirtyEditorState GetDirtyEditorStateToHistory(Mock<IProjectHistoryService> history)
		{
			var editorState = new DirtyEditorState(
				Mock.Of<IProjectLoadService>(),
				history.Object,
				Mock.Of<IProjectSaveService>(),
				Mock.Of<IProjectContext>(),
				Mock.Of<IProjectModelFactory>(),
				Mock.Of<IEditorStateChanger>());
			return editorState;
		}
		
		[Fact]
		public void 与えたIProjectSaveServiceでプロジェクトを保存できる()
		{
			var saveService = GetMockSaveService();
			var editorState = GetMockEditorStateToSave(saveService);

			editorState.SaveAsync().Wait();

			saveService.Verify(x => x.SaveAsync(), Times.Once);
		}

		[Fact]
		public void 与えたIProjectSaveServiceでプロジェクトを上書き保存できる()
		{
			var saveService = GetMockSaveService();
			var editorState = GetMockEditorStateToSave(saveService);

			editorState.SaveAsAsync().Wait();

			saveService.Verify(x => x.SaveAsAsync(), Times.Once);
		}

		[Fact]
		public void 与えたIProjectSaveServiceでプロジェクトをエクスポートできる()
		{
			var saveService = GetMockSaveService();
			var editorState = GetMockEditorStateToSave(saveService);

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

		private static DirtyEditorState GetMockEditorStateToSave(Mock<IProjectSaveService> saveService)
		{
			return new DirtyEditorState(
				Mock.Of<IProjectLoadService>(),
				Mock.Of<IProjectHistoryService>(),
				saveService.Object,
				Mock.Of<IProjectContext>(),
				Mock.Of<IProjectModelFactory>(),
				Mock.Of<IEditorStateChanger>());
		}

		[Fact]
		public void OnEditを呼ぶとUndoが更新される()
		{
			var history = CreateHistoryMock();
			var editorState = GetDirtyEditorStateToHistory(history);
			var commandAvailability = new CommandAvailability();

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
			var editorState = GetDirtyEditorStateToHistory(history);
			var commandAvailability = new CommandAvailability();

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
				Mock.Of<IEditorStateChanger>());

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
				Mock.Of<IEditorStateChanger>());

			var _ = editorState.OpenAsync().Result;

			loadService.Verify(x => x.OpenAsync(), Times.Once);
		}
	}
}
