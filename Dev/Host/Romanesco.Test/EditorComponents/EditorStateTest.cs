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
			var editorState = GetDirtyEditorState(historyService: CreateHistoryMock());
			var availability = new CommandAvailability(editorState, Mock.Of<IEditorStateRepository>());

			using var once = availability.Observable
				.Where(x => x.command == type)
				.ExpectAtLeastOnce();

			availability.UpdateCanExecute(editorState.GetHistoryService());
		}

		private static DirtyEditorState GetDirtyEditorState(Mock<IProjectLoadService>? loadService = null,
			Mock<IProjectSaveService>? saveService = null,
			Mock<IProjectHistoryService>? historyService = null)
		{
			return new DirtyEditorState(
				loadService?.Object ?? Mock.Of<IProjectLoadService>(),
				historyService?.Object ?? Mock.Of<IProjectHistoryService>(),
				saveService?.Object ?? Mock.Of<IProjectSaveService>(),
				Mock.Of<IProjectContext>(),
				Mock.Of<IProjectModelFactory>(),
				Mock.Of<IEditorStateChanger>());
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
