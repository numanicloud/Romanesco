using System;
using Moq;
using Romanesco.Model.Commands;
using Romanesco.Model.EditorComponents;
using Romanesco.Model.Services.History;
using Romanesco.Test.Helpers;
using Xunit;
using Xunit.Sdk;
using static Romanesco.Model.EditorComponents.EditorCommandType;

namespace Romanesco.Test.Commands
{
	public class CommandAvailabilityTest
	{
		[Theory]
		[InlineData(Create)]
		[InlineData(Open)]
		[InlineData(Save)]
		[InlineData(SaveAs)]
		[InlineData(Export)]
		[InlineData(Undo)]
		[InlineData(Redo)]
		public void コマンドの実行可能性が通知される(EditorCommandType type)
		{
			var availability = new CommandAvailability();

			var stream = type switch
			{
				Create => availability.CanCreate,
				Open => availability.CanOpen,
				Save => availability.CanSave,
				SaveAs => availability.CanSaveAs,
				Export => availability.CanExport,
				Undo => availability.CanUndo,
				Redo => availability.CanRedo,
				_ => throw new NotImplementedException(),
			};

			Assert.False(stream.Value);

			availability.UpdateCanExecute(type, true);
			Assert.True(stream.Value);
			
			availability.UpdateCanExecute(type, false);
			Assert.False(stream.Value);
		}
		
		[Fact]
		public void プロジェクトを作成するサービスを実行できる()
		{
			var loadService = MockHelper.GetLoaderServiceMock();
			var editorState = MockHelper.GetEditorStateMock(loadService: loadService.Object);
			var commandAvailability = new CommandAvailability();

			_ = commandAvailability.CreateAsync(editorState.Object).Result;

			loadService.Verify(x => x.CreateAsync(), Times.Once);
		}
		
		[Fact]
		public void プロジェクトを開くサービスを実行できる()
		{
			var loadService = MockHelper.GetLoaderServiceMock();
			var editorState = MockHelper.GetEditorStateMock(loadService: loadService.Object);
			var commandAvailability = new CommandAvailability();

			_ = commandAvailability.OpenAsync(editorState.Object).Result;

			loadService.Verify(x => x.OpenAsync(), Times.Once);
		}

		[Fact]
		public void 与えたIProjectSaveServiceでプロジェクトを保存できる()
		{
			var saveService = MockHelper.GetSaveServiceMock();
			var editorState = MockHelper.GetEditorStateMock(saveService: saveService.Object);
			var availability = new CommandAvailability();

			availability.SaveAsync(editorState.Object).Wait();

			saveService.Verify(x => x.SaveAsync(), Times.Once);
		}
		

		[Fact]
		public void 与えたIProjectSaveServiceでプロジェクトを上書き保存できる()
		{
			var saveService = MockHelper.GetSaveServiceMock();
			var editorState = MockHelper.GetEditorStateMock(saveService: saveService.Object);
			var availability = new CommandAvailability();

			availability.SaveAsAsync(editorState.Object).Wait();

			saveService.Verify(x => x.SaveAsAsync(), Times.Once);
		}

		[Fact]
		public void 与えたIProjectSaveServiceでプロジェクトをエクスポートできる()
		{
			var saveService = MockHelper.GetSaveServiceMock();
			var editorState = MockHelper.GetEditorStateMock(saveService: saveService.Object);
			var availability = new CommandAvailability();

			availability.ExportAsync(editorState.Object).Wait();

			saveService.Verify(x => x.ExportAsync(), Times.Once);
		}

		[Fact]
		public void Undoを実行できる()
		{
			var historyService = MockHelper.CreateHistoryMock();
			var editorState = MockHelper.GetEditorStateMock(historyService: historyService.Object);
			var availability = new CommandAvailability();

			availability.Undo(editorState.Object);

			historyService.Verify(x => x.Undo(), Times.Once);
		}
	}
}
