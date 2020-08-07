using System;
using System.Reactive.Linq;
using Moq;
using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.EditorComponents;
using Romanesco.Model.EditorComponents.States;
using Romanesco.Model.Services.History;
using Romanesco.Model.Services.Load;
using Xunit;

namespace Romanesco.Test.EditorComponents
{
	public class EditorTest
	{
		private IEditorStateChanger neverEditorStateChanger;

		public EditorTest()
		{
			var editorStateChanger = new Mock<IEditorStateChanger>();
			editorStateChanger.Setup(x => x.OnChange)
				.Returns(() => Observable.Never<EditorState>());
			this.neverEditorStateChanger = editorStateChanger.Object;
		}

		[Fact]
		public void プロジェクトを作成する命令をエディターが現在のステートに割り振る()
		{
			// Arrange
			var loadService = new Mock<IProjectLoadService>();
			loadService.Setup(x => x.CreateAsync())
				.Returns(async () => null);

			var editorState = new Mock<IEditorState>();
			editorState.Setup(x => x.GetLoadService()).Returns(loadService.Object);
			editorState.Setup(x => x.UpdateCanExecute(It.IsAny<IObserver<(EditorCommandType, bool)>>()))
				.Callback(() => { });

			var editor = new Editor(neverEditorStateChanger, editorState.Object);

			// Act
			var projectContext = editor.CreateAsync().Result;

			// Assert
			loadService.Verify(x => x.CreateAsync(), Times.Once);
		}

		[Fact]
		public void プロジェクトを開く命令をエディターが現在のステートに割り振る()
		{
			var loadService = new Mock<IProjectLoadService>();
			loadService.Setup(x => x.OpenAsync())
				.Returns(async () => null);
			
			var editorState = new Mock<IEditorState>();
			editorState.Setup(x => x.GetLoadService()).Returns(loadService.Object);
			editorState.Setup(x => x.UpdateCanExecute(It.IsAny<IObserver<(EditorCommandType, bool)>>()))
				.Callback(() => { });

			var editor = new Editor(neverEditorStateChanger, editorState.Object);

			var projectContext = editor.OpenAsync().Result;

			loadService.Verify(x => x.OpenAsync(), Times.Once);
		}

		[Fact]
		public void Undo不能になったときにイベントが発行される()
		{
			var canUndo = true;

			var history = new Mock<IProjectHistoryService>();
			history.Setup(x => x.Undo())
				.Callback(() => canUndo = false);
			history.Setup(x => x.CanUndo)
				.Returns(() => canUndo);

			var editorState = new Mock<IEditorState>();
			editorState.Setup(x => x.GetHistoryService())
				.Returns(history.Object);

			var editor = new Editor(neverEditorStateChanger, editorState.Object);

			bool raised = false;
			var disposable = editor.CanExecuteObservable.Where(x => x.command == EditorCommandType.Undo)
				.Where(x => !x.canExecute)
				.Subscribe(x => raised = true);

			editor.Undo();
			disposable.Dispose();

			Assert.True(raised);
		}

		[Fact]
		public void Redo不能になったときにイベントが発行される()
		{
			var canRedo = true;

			var history = new Mock<IProjectHistoryService>();
			history.Setup(x => x.Redo())
				.Callback(() => canRedo = false);
			history.Setup(x => x.CanRedo)
				.Returns(() => canRedo);

			var editorState = new Mock<IEditorState>();
			editorState.Setup(x => x.GetHistoryService())
				.Returns(history.Object);

			var editor = new Editor(neverEditorStateChanger, editorState.Object);

			bool raised = false;
			var disposable = editor.CanExecuteObservable.Where(x => x.command == EditorCommandType.Redo)
				.Where(x => !x.canExecute)
				.Subscribe(x => raised = true);

			editor.Redo();
			disposable.Dispose();

			Assert.True(raised);
		}

		public void Undo可能になったときにイベントが発行される()
		{
			var okHistory = new Mock<IProjectHistoryService>();
			okHistory.Setup(x => x.CanUndo).Returns(true);

			var ngHistory = new Mock<IProjectHistoryService>();
			ngHistory.Setup(x => x.CanUndo).Returns(false);

			IProjectHistoryService currentHistory = ngHistory.Object;
			var editorState = new Mock<IEditorState>();
			editorState.Setup(x => x.OnEdit())
				.Callback(() => currentHistory = okHistory.Object);
			editorState.Setup(x => x.GetHistoryService())
				.Returns(() => currentHistory);

			// いったん、OnEditをSubscribeさせないといけない
			// - ProjectContext がモック可能である必要がある
			// - あるいは、OnEditイベントを挿し込めるようにする

			var editor = new Editor(neverEditorStateChanger, editorState.Object);
		}
	}
}
