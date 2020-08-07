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
			var editorState = CreateDirtyEditorState(CreateHistoryMock());
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
			var editorState = CreateDirtyEditorState(CreateHistoryMock());
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
			var editorState = CreateDirtyEditorState(CreateHistoryMock());
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

		private static DirtyEditorState CreateDirtyEditorState(Mock<IProjectHistoryService> history)
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
	}
}
