using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Romanesco.Model.Commands;
using Romanesco.Model.EditorComponents;
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
		public void コマンドの実行可能性が通知される(EditorCommandType type)
		{
			var editorState = MockHelper.GetEditorStateMock();
			var availability = new CommandAvailability(editorState.Object);

			ICommandModel command = type switch
			{
				Create => new CreateCommand(availability.CanCreate, editorState.Object),
				Open => new OpenCommand(availability.CanOpen, editorState.Object),
				Save => new SaveCommand(availability.CanSave, editorState.Object),
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
			var loadService = MockHelper.GetLoaderServiceMock();
			var editorState = MockHelper.GetEditorStateMock(loadService: loadService.Object);
			var commandAvailability = new CommandAvailability(editorState.Object);
			var command = new CreateCommand(commandAvailability.CanCreate, editorState.Object);

			_ = command.Execute().Result;

			loadService.Verify(x => x.CreateAsync(), Times.Once);
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
	}
}
