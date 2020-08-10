using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Moq;
using Romanesco.Model.Commands;
using Romanesco.Model.EditorComponents;
using Romanesco.Model.EditorComponents.States;
using Romanesco.Model.Services.Load;
using Romanesco.Test.Helpers;
using Xunit;
using static Romanesco.Model.EditorComponents.EditorCommandType;
using Expression = Castle.DynamicProxy.Generators.Emitters.SimpleAST.Expression;

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
				_ => throw new NotImplementedException(),
			};

			Assert.False(command.CanExecute.Value);

			availability.UpdateCanExecute(type, true);
			Assert.True(command.CanExecute.Value);

			availability.UpdateCanExecute(type, false);
			Assert.False(command.CanExecute.Value);
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
			var loadService = MockHelper.GetLoaderServiceMock();
			var editorState = MockHelper.GetEditorStateMock(loadService: loadService.Object);
			var commandAvailability = new CommandAvailability(editorState.Object);
			var command = new OpenCommand(commandAvailability.CanOpen, editorState.Object);

			_ = command.Execute().Result;

			loadService.Verify(x => x.OpenAsync(), Times.Once);
		}

		[Fact]
		public void 与えたIProjectSaveServiceでプロジェクトを保存できる()
		{
			var saveService = MockHelper.GetSaveServiceMock();
			var editorState = MockHelper.GetEditorStateMock(saveService: saveService.Object);
			var availability = new CommandAvailability(editorState.Object);
			var command = new SaveCommand(availability.CanSave, editorState.Object);

			command.Execute().Wait();

			saveService.Verify(x => x.SaveAsync(), Times.Once);
		}

		[Fact]
		public void 与えたIProjectSaveServiceでプロジェクトを上書き保存できる()
		{
			var saveService = MockHelper.GetSaveServiceMock();
			var editorState = MockHelper.GetEditorStateMock(saveService: saveService.Object);
			var availability = new CommandAvailability(editorState.Object);
			var command = new SaveAsCommand(availability.CanSave, editorState.Object);

			command.Execute().Wait();

			saveService.Verify(x => x.SaveAsAsync(), Times.Once);
		}

		[Fact]
		public void 与えたIProjectSaveServiceでプロジェクトをエクスポートできる()
		{
			var saveService = MockHelper.GetSaveServiceMock();
			var editorState = MockHelper.GetEditorStateMock(saveService: saveService.Object);
			var availability = new CommandAvailability(editorState.Object);
			var command = new ExportCommand(availability.CanExport, editorState.Object);

			command.Execute().Wait();

			saveService.Verify(x => x.ExportAsync(), Times.Once);
		}
	}
}
