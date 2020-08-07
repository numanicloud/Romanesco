using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Moq;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
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
		private readonly IEditorStateChanger neverEditorStateChanger;

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

		[Theory]
		[InlineData(EditorCommandType.Undo)]
		[InlineData(EditorCommandType.Redo)]
		internal void UndoまたはRedo可能になったときにイベントが発行される(EditorCommandType commandType)
		{
			// Arrange
			var editSubject = new Subject<Unit>();
			var loader = new Mock<IProjectLoadService>();
			{
				var rootState = new Mock<IFieldState>();
				rootState.Setup(x => x.OnEdited)
					.Returns(editSubject);

				var project = new Mock<IProject>();
				project.Setup(x => x.Root)
					.Returns(() => new StateRoot(new object(), new []{ rootState.Object }));

				var projectContext = new ProjectContext(project.Object, new Mock<IProjectTypeExporter>().Object);

				loader.Setup(x => x.CreateAsync())
					.Returns(async () => projectContext);
			}

			/* ここでぶったぎりたい */

			var editorState = new Mock<IEditorState>();
			{
				var okHistory = new Mock<IProjectHistoryService>();
				okHistory.Setup(x => x.CanUndo).Returns(true);
				okHistory.Setup(x => x.CanRedo).Returns(true);

				var ngHistory = new Mock<IProjectHistoryService>();
				ngHistory.Setup(x => x.CanUndo).Returns(false);
				ngHistory.Setup(x => x.CanRedo).Returns(false);

				IProjectHistoryService currentHistory = ngHistory.Object;
				editorState.Setup(x => x.OnEdit())
					.Callback(() => currentHistory = okHistory.Object);
				editorState.Setup(x => x.GetHistoryService())
					.Returns(() => currentHistory);
				editorState.Setup(x => x.GetLoadService())
					.Returns(loader.Object);
			}

			var editor = new Editor(neverEditorStateChanger, editorState.Object);

			bool raised = false;
			editor.CanExecuteObservable.Where(x => x.command == commandType)
				.Where(x => x.canExecute)
				.Subscribe(x => raised = true);

			// Act
			var projectResult = editor.CreateAsync().Result;
			editSubject.OnNext(Unit.Default);

			// Assert
			Assert.True(raised);
		}
	}
}
