﻿using System;
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
			var fixture = GetProjectCreatingFixture();

			_ = fixture.Item3.CreateAsync().Result;

			fixture.Item1.Verify(x => x.CreateAsync(), Times.Once);
		}

		[Fact]
		public void プロジェクトが作成されるとHistoryの更新をステートに要求する()
		{
			var editSubject = new Subject<Unit>();
			var fixture = GetProjectCreatingFixture(editSubject);

			_ = fixture.Item3.CreateAsync().Result;
			editSubject.OnNext(Unit.Default);

			fixture.Item2.Verify(x => x.NotifyEdit(), Times.Once);
		}

		private (Mock<IProjectLoadService>, Mock<IEditorState>, Editor) GetProjectCreatingFixture(Subject<Unit>? onEdit = null)
		{
			var loader = MockHelper.GetLoaderServiceMock(onEdit is null ? null : GetContext().Object);
			var editorState = MockHelper.GetEditorStateMock(loadService: loader.Object);
			var editor = new Editor(neverEditorStateChanger, editorState.Object, new CommandAvailability());
			return (loader, editorState, editor);

			Mock<IProjectContext> GetContext()
			{
				var context = new Mock<IProjectContext>();
				context.Setup(x => x.ObserveEdit(It.IsAny<Action>()))
					.Returns((Action action) => onEdit.Subscribe(x => action()));
				return context;
			}

			Mock<IEditorState> GetEditorState()
			{
				var mock = new Mock<IEditorState>();
				if (loader is { })
				{
					mock.Setup(x => x.GetLoadService())
						.Returns(loader.Object);
				}
				mock.Setup(x => x.NotifyEdit())
					.Callback(() => { });
				return mock;
			}
		}

		[Fact]
		public void プロジェクトを開く命令をエディターが現在のステートに割り振る()
		{
			var loadService = new Mock<IProjectLoadService>();
			loadService.Setup(x => x.OpenAsync())
				.Returns(async () => null);

			var editorState = new Mock<IEditorState>();
			editorState.Setup(x => x.GetLoadService())
				.Returns(loadService.Object);

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
