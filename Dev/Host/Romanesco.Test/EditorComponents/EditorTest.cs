using System.Reactive.Linq;
using Moq;
using Romanesco.Model.EditorComponents;
using Romanesco.Model.EditorComponents.States;
using Romanesco.Test.Helpers;
using Xunit;

namespace Romanesco.Test.EditorComponents
{
	public class EditorTest
	{
		private readonly IEditorStateChanger neverEditorStateChanger;

		public EditorTest()
		{
			var editorStateChanger = new Mock<IEditorStateChanger>();
			editorStateChanger.Setup(x => x.OnChange)
				.Returns(() => Observable.Never<EditorState>());
			this.neverEditorStateChanger = editorStateChanger.Object;
		}

		[Fact]
		public void ステートが切り替わるとコマンドの実行可能性も更新される()
		{
			var okHistory = MockHelper.CreateHistoryMock(canUndo: true, canRedo: true);
			var okState = MockHelper.GetEditorStateMock(historyService: okHistory.Object);

			var ngHistory = MockHelper.CreateHistoryMock(canUndo: false, canRedo: false);
			var ngState = MockHelper.GetEditorStateMock(historyService: ngHistory.Object);

			var editor = new Editor(neverEditorStateChanger, okState.Object);

			Assert.True(editor.CommandAvailabilityPublisher.CanUndo.Value);
			Assert.True(editor.CommandAvailabilityPublisher.CanRedo.Value);

			editor.ChangeState(ngState.Object);
			
			Assert.False(editor.CommandAvailabilityPublisher.CanUndo.Value);
			Assert.False(editor.CommandAvailabilityPublisher.CanRedo.Value);
		}
	}
}
