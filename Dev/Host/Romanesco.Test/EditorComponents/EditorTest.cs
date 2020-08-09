using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Moq;
using Romanesco.Common.Model.ProjectComponent;
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
		public void プロジェクトを作成する命令をエディターが現在のステートに割り振る()
		{
			var loader = MockHelper.GetLoaderServiceMock();
			var editorState = MockHelper.GetEditorStateMock(loadService: loader.Object);
			var editor = new Editor(neverEditorStateChanger, editorState.Object);

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
			editorState.Setup(x => x.OnEdit()).Callback(() => { });

			var editor = new Editor(neverEditorStateChanger, editorState.Object);

			_ = editor.CreateAsync().Result;
			editSubject.OnNext(Unit.Default);


			editorState.Verify(x => x.OnEdit(), Times.Once);
		}

		[Fact]
		public void プロジェクトを開く命令をエディターが現在のステートに割り振る()
		{
			var loadService = MockHelper.GetLoaderServiceMock();
			var editorState = MockHelper.GetEditorStateMock(loadService: loadService.Object);
			var editor = new Editor(neverEditorStateChanger, editorState.Object);

			var _ = editor.OpenAsync().Result;

			loadService.Verify(x => x.OpenAsync(), Times.Once);
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
