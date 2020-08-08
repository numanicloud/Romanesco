using System;
using System.Collections.Generic;
using System.Text;
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
			var loadService = new Mock<IProjectLoadService>();
			loadService.Setup(x => x.CreateAsync())
				.Returns(async () => null);

			var editorState = GetDirtyEditorState(loadService: loadService);
			var commandAvailability = new CommandAvailability();

			_ = commandAvailability.CreateAsync(editorState).Result;

			loadService.Verify(x => x.CreateAsync(), Times.Once);
		}

		private static DirtyEditorState GetDirtyEditorState(CommandAvailability? commandAvailability = null,
			Mock<IProjectLoadService>? loadService = null,
			Mock<IProjectSaveService>? saveService = null,
			Mock<IProjectHistoryService>? historyService = null)
		{
			var editorSession = new EditorSession(
				Mock.Of<IEditorStateChanger>(),
				commandAvailability ?? new CommandAvailability());

			return new DirtyEditorState(
				loadService?.Object ?? Mock.Of<IProjectLoadService>(),
				historyService?.Object ?? Mock.Of<IProjectHistoryService>(),
				saveService?.Object ?? Mock.Of<IProjectSaveService>(),
				Mock.Of<IProjectContext>(),
				Mock.Of<IProjectModelFactory>(),
				editorSession);
		}
	}
}
