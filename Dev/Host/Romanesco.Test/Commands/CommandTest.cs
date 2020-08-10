using System;
using System.Linq.Expressions;
using Moq;
using Romanesco.Model.Commands;
using Romanesco.Model.EditorComponents;
using Romanesco.Model.EditorComponents.States;
using Romanesco.Model.Services.History;
using Romanesco.Model.Services.Load;
using Romanesco.Model.Services.Save;
using Romanesco.Test.Helpers;
using Xunit;
using static Romanesco.Model.EditorComponents.EditorCommandType;

namespace Romanesco.Test.Commands
{
	public class CommandTest
	{
		[Theory]
		[InlineData(Create)]
		[InlineData(Open)]
		[InlineData(Save)]
		[InlineData(SaveAs)]
		[InlineData(Export)]
		[InlineData(Undo)]
		public void コマンドの実行可能性が通知される(EditorCommandType type)
		{
			var editorState = MockHelper.GetEditorStateMock();
			var availability = new CommandAvailability(editorState.Object);

			ICommandModel command = type switch
			{
				Create => new CreateCommand(availability.CanCreate, editorState.Object),
				Open => new OpenCommand(availability.CanOpen, editorState.Object),
				Save => new SaveCommand(availability.CanSave, editorState.Object),
				SaveAs => new SaveAsCommand(availability.CanSaveAs, editorState.Object),
				Export => new ExportCommand(availability.CanExport, editorState.Object),
				Undo => new UndoCommand(availability.CanUndo, editorState.Object),
				_ => throw new NotImplementedException(),
			};

			Assert.False(command.CanExecute.Value);

			availability.UpdateCanExecute(type, true);
			Assert.True(command.CanExecute.Value);

			availability.UpdateCanExecute(type, false);
			Assert.False(command.CanExecute.Value);
		}

		[Fact]
		public void プロジェクトを作成するサービスを実行できる()
		{
			AssertCommandExecution(x => x.CreateAsync(),
				(p, s) =>
				{
					var command = new CreateCommand(p.CanCreate, s);
					_ = command.Execute().Result;
				});
		}

		[Fact]
		public void プロジェクトを開くサービスを実行できる()
		{
			AssertCommandExecution(x => x.OpenAsync(),
				(p, s) =>
				{
					var command = new OpenCommand(p.CanOpen, s);
					_ = command.Execute().Result;
				});
		}

		[Fact]
		public void 与えたIProjectSaveServiceでプロジェクトを保存できる()
		{
			AssertCommandExecution(x => x.SaveAsync(),
				(p, s) =>
				{
					var command = new SaveCommand(p.CanSave, s);
					command.Execute().Wait();
				});
		}

		[Fact]
		public void 与えたIProjectSaveServiceでプロジェクトを上書き保存できる()
		{
			AssertCommandExecution(x => x.SaveAsAsync(),
				(p, s) =>
				{
					var command = new SaveAsCommand(p.CanSaveAs, s);
					command.Execute().Wait();
				});
		}

		[Fact]
		public void 与えたIProjectSaveServiceでプロジェクトをエクスポートできる()
		{
			AssertCommandExecution(x => x.ExportAsync(),
				(p, s) =>
				{
					var command = new ExportCommand(p.CanExport, s);
					command.Execute().Wait();
				});
		}

		[Fact]
		public void Undoを呼び出せる()
		{
			AssertCommandExecution(x => x.Undo(),
				(p, s) =>
				{
					var command = new UndoCommand(p.CanUndo, s);
					command.Execute();
				});
		}

		private static void AssertCommandExecution<TCommandResult>(
			Expression<Func<IProjectLoadService, TCommandResult>> methodToVerify,
			Action<CommandAvailability, IEditorState> execution)
		{
			var loadService = MockHelper.GetLoaderServiceMock();
			var editorState = MockHelper.GetEditorStateMock(loadService: loadService.Object);
			var commandAvailability = new CommandAvailability(editorState.Object);

			execution(commandAvailability, editorState.Object);
			
			loadService.Verify(methodToVerify, Times.Once);
		}
		
		private static void AssertCommandExecution<TCommandResult>(
			Expression<Func<IProjectSaveService, TCommandResult>> methodToVerify,
			Action<CommandAvailability, IEditorState> execution)
		{
			var saveService = MockHelper.GetSaveServiceMock();
			var editorState = MockHelper.GetEditorStateMock(saveService: saveService.Object);
			var commandAvailability = new CommandAvailability(editorState.Object);

			execution(commandAvailability, editorState.Object);
			
			saveService.Verify(methodToVerify, Times.Once);
		}
		
		private static void AssertCommandExecution(
			Expression<Action<IProjectHistoryService>> methodToVerify,
			Action<CommandAvailability, IEditorState> execution)
		{
			var historyService = MockHelper.CreateHistoryMock();
			var editorState = MockHelper.GetEditorStateMock(historyService: historyService.Object);
			var commandAvailability = new CommandAvailability(editorState.Object);

			execution(commandAvailability, editorState.Object);
			
			historyService.Verify(methodToVerify, Times.Once);
		}
	}
}
