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
		public void コマンドの実行可能性が通知される(EditorCommandType type)
		{
			var editorState = MockHelper.GetEditorStateMock();
			var availability = new CommandAvailability(editorState.Object);

			ICommandModel command = type switch
			{
				Create => new CreateCommand(availability.CanCreate, editorState.Object),
				Open => new OpenCommand(availability.CanOpen, editorState.Object),
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
	}
}
