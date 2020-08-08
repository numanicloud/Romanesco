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
using Romanesco.Test.Helpers;
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
			var loader = MockHelper.GetLoaderServiceMock();
			var editorState = MockHelper.GetEditorStateMock(loadService: loader.Object);
			var editor = new Editor(neverEditorStateChanger, editorState.Object, new CommandAvailability());

			_ = editor.CreateAsync().Result;

			loader.Verify(x => x.CreateAsync(), Times.Once);
		}

		[Fact]
		public void プロジェクトが作成されるとHistoryの更新をステートに要求する()
		{
			var editSubject = new Subject<Unit>();
			var context = new Mock<IProjectContext>();
			context.Setup(x => x.ObserveEdit(It.IsAny<Action>()))
				.Returns((Action action) => editSubject.Subscribe(x => action()));

			var loader = MockHelper.GetLoaderServiceMock(context.Object);
			var editorState = MockHelper.GetEditorStateMock(loadService: loader.Object);
			var editor = new Editor(neverEditorStateChanger, editorState.Object, new CommandAvailability());

			_ = editor.CreateAsync().Result;
			editSubject.OnNext(Unit.Default);

			editorState.Verify(x => x.NotifyEdit(), Times.Once);
		}

		[Fact]
		public void プロジェクトを開く命令をエディターが現在のステートに割り振る()
		{
			var loadService = MockHelper.GetLoaderServiceMock();
			var editorState = MockHelper.GetEditorStateMock(loadService: loadService.Object);
			var editor = new Editor(neverEditorStateChanger, editorState.Object, new CommandAvailability());

			var _ = editor.OpenAsync().Result;

			loadService.Verify(x => x.OpenAsync(), Times.Once);
		}

		[Fact]
		public void UndoするとUndo可能性が更新される()
		{
			var commandAvailability = new CommandAvailability();

			var editorState = new Mock<IEditorState>();
			editorState.Setup(x => x.Undo())
				.Callback(() => commandAvailability.UpdateCanExecute(Undo, false));

			var editor = new Editor(neverEditorStateChanger, editorState.Object, commandAvailability);

			using var once = editor.CanExecuteObservable
				.ExpectAtLeastOnce();

			editor.Undo();
		}
		
		[Fact]
		public void RedoするとRedo可能性が更新される()
		{
			var commandAvailability = new CommandAvailability();

			var editorState = new Mock<IEditorState>();
			editorState.Setup(x => x.Redo())
				.Callback(() => commandAvailability.UpdateCanExecute(Redo, false));

			var editor = new Editor(neverEditorStateChanger, editorState.Object, commandAvailability);

			using var once = editor.CanExecuteObservable
				.ExpectAtLeastOnce();

			editor.Redo();
		}
	}
}
