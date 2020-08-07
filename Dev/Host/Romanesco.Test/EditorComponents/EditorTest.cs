using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Moq;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.Commands;
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
			editorState.Setup(x => x.UpdateCanExecute(It.IsAny<IObserver<(EditorCommandType, bool)>>(), It.IsAny<CommandAvailability>()))
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
			editorState.Setup(x => x.UpdateCanExecute(It.IsAny<IObserver<(EditorCommandType, bool)>>(), It.IsAny<CommandAvailability>()))
				.Callback(() => { });

			var editor = new Editor(neverEditorStateChanger, editorState.Object);

			var projectContext = editor.OpenAsync().Result;

			loadService.Verify(x => x.OpenAsync(), Times.Once);
		}

		[Fact]
		public void プロジェクトが作成されるとHistoryの更新をステートに要求する()
		{
			// これはもはやEditorStateのテストかも
			// Editor.OnEdit の中身を EditorState で行うようにすればいいかな

			var editSubject = new Subject<Unit>();
			var loader = new Mock<IProjectLoadService>();
			{
				var iprojectContext = new Mock<IProjectContext>();
				iprojectContext.Setup(x => x.ObserveEdit(It.IsAny<Action>()))
					.Returns((Action action) => editSubject.Subscribe(x => action()));

				loader.Setup(x => x.CreateAsync())
					.Returns(async () => iprojectContext.Object);
			}

			var editorState = new Mock<IEditorState>();
			editorState.Setup(x => x.OnEdit()).Callback(() => { });
			editorState.Setup(x => x.GetLoadService()).Returns(loader.Object);
			editorState.Setup(x => x.UpdateHistoryAvailability(It.IsAny<IObserver<(EditorCommandType, bool)>>(), It.IsAny<CommandAvailability>()))
				.Callback(() => { });
			
			var editor = new Editor(neverEditorStateChanger, editorState.Object);
			var projectResult = editor.CreateAsync().Result;
			editSubject.OnNext(Unit.Default);

			editorState.Verify(x => x.UpdateHistoryAvailability(It.IsAny<IObserver<(EditorCommandType, bool)>>(), It.IsAny<CommandAvailability>()), Times.Once);
		}

		[Theory]
		[InlineData(EditorCommandType.Undo)]
		[InlineData(EditorCommandType.Redo)]
		internal void UndoまたはRedo可能になったときにイベントが発行される(EditorCommandType commandType)
		{
			// 2つの部分に分解できるテスト
			// - プロジェクトが作成されると、Undo/Redoの状態が更新される
			// - Undo/Redoが可能になるとイベントを発行する

			// これはCanExecuteObservableのテストということになるが、
			// CanExecuteObservableは他のメンバーにリダイレクトしてるだけなので不要かも？

			// Arrange
			var editSubject = new Subject<Unit>();
			var loader = new Mock<IProjectLoadService>();
			{
				var iprojectContext = new Mock<IProjectContext>();
				iprojectContext.Setup(x => x.ObserveEdit(It.IsAny<Action>()))
					.Returns((Action action) => editSubject.Subscribe(x => action()));

				loader.Setup(x => x.CreateAsync())
					.Returns(async () => iprojectContext.Object);
			}

			var editorState = new Mock<IEditorState>();
			editorState.Setup(x => x.OnEdit()).Callback(() => { });
			editorState.Setup(x => x.GetLoadService()).Returns(loader.Object);
			editorState.Setup(x => x.UpdateHistoryAvailability(It.IsAny<IObserver<(EditorCommandType, bool)>>(), It.IsAny<CommandAvailability>()))
				.Callback((IObserver<(EditorCommandType, bool)> observer, CommandAvailability av) =>
				{
					observer.OnNext((EditorCommandType.Undo, true));
					observer.OnNext((EditorCommandType.Redo, true));
				});

			var editor = new Editor(neverEditorStateChanger, editorState.Object);

			var raised = false;
			editor.CanExecuteObservable.Where(x => x.command == commandType)
				.Where(x => x.canExecute)
				.Subscribe(x => raised = true);

			// Act
			var projectResult = editor.CreateAsync().Result;
			editSubject.OnNext(Unit.Default);

			// Assert
			Assert.True(raised);
		}

		[Fact]
		public void UndoするとUndo可能性が更新される()
		{
			var editorState = new Mock<IEditorState>();
			editorState.Setup(x => x.Undo(It.IsAny<IObserver<(EditorCommandType, bool)>>(), It.IsAny<CommandAvailability>()))
				.Callback((IObserver<(EditorCommandType, bool)> observer, CommandAvailability av) =>
				{
					observer.OnNext((EditorCommandType.Undo, false));
				});

			var editor = new Editor(neverEditorStateChanger, editorState.Object);

			bool raised = false;
			using var disposable = editor.CanExecuteObservable
				.Subscribe(x => raised = true);

			editor.Undo();

			Assert.True(raised);
		}
		
		[Fact]
		public void RedoするとRedo可能性が更新される()
		{
			var editorState = new Mock<IEditorState>();
			editorState.Setup(x => x.Redo(It.IsAny<IObserver<(EditorCommandType, bool)>>(), It.IsAny<CommandAvailability>()))
				.Callback((IObserver<(EditorCommandType, bool)> observer, CommandAvailability av) =>
				{
					observer.OnNext((EditorCommandType.Redo, false));
				});

			var editor = new Editor(neverEditorStateChanger, editorState.Object);

			bool raised = false;
			using var disposable = editor.CanExecuteObservable
				.Subscribe(x => raised = true);

			editor.Redo();

			Assert.True(raised);
		}
	}
}
