using System;
using System.Collections.Generic;
using System.Linq;
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
using Romanesco.Test.Helpers;

namespace Romanesco.Test.EditorComponents
{
	public class EditorStateTest
	{
		[Fact]
		public void Undoを呼ぶとUndo可能性が更新される()
		{
			var availability = new CommandAvailability();
			var editorState = GetDirtyEditorState(availability, historyService: CreateHistoryMock());

			using var once = availability.Observable
				.Where(x => x.command == EditorCommandType.Undo)
				.ExpectAtLeastOnce();

			editorState.Undo();
		}
		
		[Fact]
		public void Redoを呼ぶとRedo可能性が更新される()
		{
			var availability = new CommandAvailability();
			var editorState = GetDirtyEditorState(availability, historyService: CreateHistoryMock());

			using var once = availability.Observable
				.Where(x => x.command == EditorCommandType.Redo)
				.ExpectAtLeastOnce();

			editorState.Redo();
		}
		
		[Theory]
		[InlineData(EditorCommandType.Undo)]
		[InlineData(EditorCommandType.Redo)]
		public void UndoとRedoの状態を更新できる(EditorCommandType type)
		{
			// これはもはや CommandAvailability のテスト
			var availability = new CommandAvailability();
			var editorState = GetDirtyEditorState(availability, historyService: CreateHistoryMock());

			using var once = availability.Observable
				.Where(x => x.command == type)
				.ExpectAtLeastOnce();

			availability.UpdateCanExecute(editorState.GetHistoryService());
		}

		[Fact]
		public void 与えたIProjectSaveServiceでプロジェクトを保存できる()
		{
			var saveService = GetMockSaveService();
			var editorState = GetDirtyEditorState(new CommandAvailability(), saveService: saveService);

			editorState.SaveAsync().Wait();

			saveService.Verify(x => x.SaveAsync(), Times.Once);
		}

		[Fact]
		public void 与えたIProjectSaveServiceでプロジェクトを上書き保存できる()
		{
			var saveService = GetMockSaveService();
			var editorState = GetDirtyEditorState(new CommandAvailability(), saveService: saveService);

			editorState.SaveAsAsync().Wait();

			saveService.Verify(x => x.SaveAsAsync(), Times.Once);
		}

		[Fact]
		public void 与えたIProjectSaveServiceでプロジェクトをエクスポートできる()
		{
			var saveService = GetMockSaveService();
			var editorState = GetDirtyEditorState(new CommandAvailability(), saveService: saveService);

			editorState.ExportAsync().Wait();

			saveService.Verify(x => x.ExportAsync(), Times.Once);
		}

		[Fact]
		public void OnEditを呼ぶとUndoが更新される()
		{
			var history = CreateHistoryMock();
			var commandAvailability = new CommandAvailability();
			var editorState = GetDirtyEditorState(commandAvailability, historyService: history);

			using var once = commandAvailability.Observable
				.Where(x => x.command == EditorCommandType.Undo)
				.ExpectAtLeastOnce();

			editorState.NotifyEdit();
		}

		[Fact]
		public void OnEditを呼ぶとRedoが更新される()
		{
			var history = CreateHistoryMock();
			var commandAvailability = new CommandAvailability();
			var editorState = GetDirtyEditorState(commandAvailability, historyService: history);

			using var once = commandAvailability.Observable
				.Where(x => x.command == EditorCommandType.Redo)
				.ExpectAtLeastOnce();

			editorState.NotifyEdit();
		}

		[Fact]
		public void プロジェクトを作成するサービスを実行できる()
		{
			var loadService = new Mock<IProjectLoadService>();
			loadService.Setup(x => x.CreateAsync())
				.Returns(async () => null);

			var editorState = GetDirtyEditorState(loadService: loadService);

			_ = editorState.CreateAsync().Result;

			loadService.Verify(x => x.CreateAsync(), Times.Once);
		}

		[Fact]
		public void プロジェクトを開くサービスを実行できる()
		{
			var loadService = new Mock<IProjectLoadService>();
			loadService.Setup(x => x.OpenAsync())
				.Returns(async () => null);

			var editorState = GetDirtyEditorState(loadService: loadService);

			_ = editorState.OpenAsync().Result;

			loadService.Verify(x => x.OpenAsync(), Times.Once);
		}

		[Fact]
		public void 全コマンドの可用性を更新できる()
		{
			IDisposable Expect(CommandAvailability ca, EditorCommandType type)
			{
				return ca.Observable.Where(x => x.command == type)
					.ExpectAtLeastOnce();
			}

			var loadService = new Mock<IProjectLoadService>();
			loadService.Setup(x => x.CanOpen).Returns(true);
			loadService.Setup(x => x.CanCreate).Returns(true);

			var saveService = new Mock<IProjectSaveService>();
			saveService.Setup(x => x.CanSave).Returns(true);
			saveService.Setup(x => x.CanExport).Returns(true);

			var historyService = new Mock<IProjectHistoryService>();
			historyService.Setup(x => x.CanUndo).Returns(true);
			historyService.Setup(x => x.CanRedo).Returns(true);

			var availability = new CommandAvailability();
			var editorState = GetDirtyEditorState(availability, loadService, saveService, historyService);

			var disposables = new[]
			{
				Expect(availability, EditorCommandType.Create),
				Expect(availability, EditorCommandType.Open),
				Expect(availability, EditorCommandType.Save),
				Expect(availability, EditorCommandType.SaveAs),
				Expect(availability, EditorCommandType.Export),
				Expect(availability, EditorCommandType.Undo),
				Expect(availability, EditorCommandType.Redo),
			};

			editorState.UpdateCanExecute();

			disposables.ForEach(x => x.Dispose());
		}

		private static DirtyEditorState GetDirtyEditorState(CommandAvailability? commandAvailability = null,
			Mock<IProjectLoadService>? loadService = null,
			Mock<IProjectSaveService>? saveService = null,
			Mock<IProjectHistoryService>? historyService = null)
		{
			return new DirtyEditorState(
				loadService?.Object ?? Mock.Of<IProjectLoadService>(),
				historyService?.Object ?? Mock.Of<IProjectHistoryService>(),
				saveService?.Object ?? Mock.Of<IProjectSaveService>(),
				Mock.Of<IProjectContext>(),
				Mock.Of<IProjectModelFactory>(),
				Mock.Of<IEditorStateChanger>(),
				commandAvailability ?? new CommandAvailability());
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

		private static Mock<IProjectSaveService> GetMockSaveService()
		{
			var saveService = new Mock<IProjectSaveService>();
			saveService.Setup(x => x.SaveAsync())
				.Callback(async () => { });
			saveService.Setup(x => x.SaveAsAsync())
				.Callback(async () => { });
			saveService.Setup(x => x.ExportAsync())
				.Callback(async () => { });
			return saveService;
		}
	}
}
