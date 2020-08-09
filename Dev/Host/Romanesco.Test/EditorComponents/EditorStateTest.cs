using System;
using System.Linq;
using System.Reactive.Linq;
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
using Romanesco.Test.Helpers;

namespace Romanesco.Test.EditorComponents
{
	public class EditorStateTest
	{
		// ここはEditorState自身のテストなので、MockHelperからIEditorStateの実装を取ってはいけない

		[Theory]
		[InlineData(EditorCommandType.Undo)]
		[InlineData(EditorCommandType.Redo)]
		public void UndoとRedoの状態を更新できる(EditorCommandType type)
		{
			// これはもはや CommandAvailability のテスト
			var availability = new CommandAvailability();
			var editorState = GetDirtyEditorState(availability, historyService: CreateHistoryMock());

			using var once = availability.Observable
				.Where(x => x.command == type)
				.ExpectAtLeastOnce();

			availability.UpdateCanExecute(editorState.GetHistoryService());
		}

		[Fact]
		public void OnEditを呼ぶとUndoが更新される()
		{
			var history = CreateHistoryMock();
			var commandAvailability = new CommandAvailability();
			var editorState = GetDirtyEditorState(commandAvailability, historyService: history);

			using var once = commandAvailability.CanUndo.ExpectAtLeastOnce();

			editorState.NotifyEdit();
		}

		[Fact]
		public void OnEditを呼ぶとRedoが更新される()
		{
			var history = CreateHistoryMock();
			var commandAvailability = new CommandAvailability();
			var editorState = GetDirtyEditorState(commandAvailability, historyService: history);

			using var once = commandAvailability.CanRedo.ExpectAtLeastOnce();

			editorState.NotifyEdit();
		}

		[Fact]
		public void 全コマンドの可用性を更新できる()
		{
			IDisposable Expect(CommandAvailability ca, EditorCommandType type)
			{
				return ca.Observable.Where(x => x.command == type)
					.ExpectAtLeastOnce();
			}

			var loadService = MockHelper.GetLoaderServiceMock(canCreate: true, canOpen: true);
			var saveService = MockHelper.GetSaveServiceMock(true, true);

			var historyService = new Mock<IProjectHistoryService>();
			historyService.Setup(x => x.CanUndo).Returns(true);
			historyService.Setup(x => x.CanRedo).Returns(true);

			var availability = new CommandAvailability();
			var editorState = GetDirtyEditorState(availability, loadService, saveService, historyService);

			var disposables = new[]
			{
				Expect(availability, EditorCommandType.Create),
				Expect(availability, EditorCommandType.Open),
				Expect(availability, EditorCommandType.Save),
				Expect(availability, EditorCommandType.SaveAs),
				Expect(availability, EditorCommandType.Export),
				Expect(availability, EditorCommandType.Undo),
				Expect(availability, EditorCommandType.Redo),
			};

			editorState.UpdateCanExecute();

			disposables.ForEach(x => x.Dispose());
		}

		private static DirtyEditorState GetDirtyEditorState(CommandAvailability? commandAvailability = null,
			Mock<IProjectLoadService>? loadService = null,
			Mock<IProjectSaveService>? saveService = null,
			Mock<IProjectHistoryService>? historyService = null)
		{
			var editorSession = new EditorSession(
				Mock.Of<IEditorStateChanger>(),
				commandAvailability ?? new CommandAvailability());

			return new DirtyEditorState(
				loadService?.Object ?? Mock.Of<IProjectLoadService>(),
				historyService?.Object ?? Mock.Of<IProjectHistoryService>(),
				saveService?.Object ?? Mock.Of<IProjectSaveService>(),
				Mock.Of<IProjectContext>(),
				Mock.Of<IProjectModelFactory>(),
				editorSession);
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
	}
}
