using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Moq;
using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.Commands;
using Romanesco.Model.EditorComponents;
using Romanesco.Model.EditorComponents.States;
using Romanesco.Model.Services.Load;
using Romanesco.Model.Services.Save;
using Xunit;
using static Romanesco.Model.EditorComponents.EditorCommandType;

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
			var editorState = new Mock<IEditorState>();
			editorState.Setup(x => x.CreateAsync())
				.Returns(async () => null);

			var editor = new Editor(neverEditorStateChanger, editorState.Object, new CommandAvailability());

			// Act
			var _ = editor.CreateAsync().Result;

			// Assert
			editorState.Verify(x => x.CreateAsync(), Times.Once);
		}

		[Fact]
		public void プロジェクトを開く命令をエディターが現在のステートに割り振る()
		{
			var editorState = new Mock<IEditorState>();
			editorState.Setup(x => x.OpenAsync())
				.Returns(async () => null);

			var editor = new Editor(neverEditorStateChanger, editorState.Object, new CommandAvailability());

			var _ = editor.OpenAsync().Result;

			editorState.Verify(x => x.OpenAsync(), Times.Once);
		}

		[Fact]
		public void プロジェクトが作成されるとHistoryの更新をステートに要求する()
		{
			// これはもはやEditorStateのテストかも

			var editSubject = new Subject<Unit>();
			var iprojectContext2 = new Mock<IProjectContext>();
			iprojectContext2.Setup(x => x.ObserveEdit(It.IsAny<Action>()))
				.Returns((Action action) => editSubject.Subscribe(x => action()));

			var editorState = new Mock<IEditorState>();
			editorState.Setup(x => x.CreateAsync())
				.Returns(async () => iprojectContext2.Object);
			editorState.Setup(x => x.NotifyEdit())
				.Callback(() => { });
			
			var editor = new Editor(neverEditorStateChanger, editorState.Object, new CommandAvailability());
			var projectResult = editor.CreateAsync().Result;
			editSubject.OnNext(Unit.Default);

			editorState.Verify(x => x.NotifyEdit(), Times.Once);
		}

		[Fact]
		public void UndoするとUndo可能性が更新される()
		{
			var commandAvailability = new CommandAvailability();

			var editorState = new Mock<IEditorState>();
			editorState.Setup(x => x.Undo())
				.Callback(() => commandAvailability.UpdateCanExecute(Undo, false));

			var editor = new Editor(neverEditorStateChanger, editorState.Object, commandAvailability);

			bool raised = false;
			using var disposable = editor.CanExecuteObservable
				.Subscribe(x => raised = true);

			editor.Undo();

			Assert.True(raised);
		}
		
		[Fact]
		public void RedoするとRedo可能性が更新される()
		{
			var commandAvailability = new CommandAvailability();

			var editorState = new Mock<IEditorState>();
			editorState.Setup(x => x.Redo())
				.Callback(() => commandAvailability.UpdateCanExecute(Redo, false));

			var editor = new Editor(neverEditorStateChanger, editorState.Object, commandAvailability);

			bool raised = false;
			using var disposable = editor.CanExecuteObservable
				.Subscribe(x => raised = true);

			editor.Redo();

			Assert.True(raised);
		}
	}
}
