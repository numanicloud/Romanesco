using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Romanesco.Model.Commands;
using Romanesco.Model.Infrastructure;
using Romanesco.Model.Services.History;
using Romanesco.Model.Services.Load;
using Romanesco.Model.Services.Save;
using Romanesco.Model.States;
using Romanesco.Test.EditorComponents;
using Xunit;

namespace Romanesco.Test.Commands
{
	public class CommandContextTest
	{
		[Fact]
		public void CreateできないステートではCreateコマンドが実行不可()
		{
			var s = new TestEditorState();
			s.LoadService.Setup(m => m.CanCreate).Returns(false);

			var context = GetContext(s);

			Assert.False(context.CanExecute(EditorCommandType.Create));
		}

		[Fact]
		public void OpenできないステートではOpenコマンドが実行不可()
		{
			var s = new TestEditorState();
			s.LoadService.Setup(m => m.CanOpen).Returns(false);

			var context = GetContext(s);

			Assert.False(context.CanExecute(EditorCommandType.Open));
		}

		[Fact]
		public void SaveできないステートではSaveコマンドが実行不可()
		{
			var s = new TestEditorState();
			s.SaveService.Setup(m => m.CanSave).Returns(false);

			var context = GetContext(s);

			Assert.False(context.CanExecute(EditorCommandType.Save));
		}

		[Fact]
		public void SaveできないステートではSaveAsコマンドが実行不可()
		{
			var s = new TestEditorState();
			s.SaveService.Setup(m => m.CanSave).Returns(false);

			var context = GetContext(s);

			Assert.False(context.CanExecute(EditorCommandType.SaveAs));
		}

		[Fact]
		public void ExportできないステートではExportコマンドが実行不可()
		{
			var s = new TestEditorState();
			s.SaveService.Setup(m => m.CanExport).Returns(false);

			var context = GetContext(s);

			Assert.False(context.CanExecute(EditorCommandType.Export));
		}

		[Fact]
		public void UndoできないステートではUndoコマンドが実行不可()
		{
			var s = new TestEditorState();
			s.HistoryService.Setup(m => m.CanUndo).Returns(false);

			var context = GetContext(s);

			Assert.False(context.CanExecute(EditorCommandType.Undo));
		}

		[Fact]
		public void RedoできないステートではRedoコマンドが実行不可()
		{
			var s = new TestEditorState();
			s.HistoryService.Setup(m => m.CanRedo).Returns(false);

			var context = GetContext(s);

			Assert.False(context.CanExecute(EditorCommandType.Redo));
		}

		private CommandContext GetContext(IEditorState state)
		{
			var editorStateChangerMock = new EditorStateChangerMock();
			editorStateChangerMock.ChangeState(state);
			
			return new CommandContext(
				new ProjectSwitcherMock(),
				editorStateChangerMock,
				Mock.Of<IModelFactory>());
		}
	}

	internal class TestEditorState : IEditorState
	{
		public Mock<IProjectLoadService> LoadService { get; } = new();
		public Mock<IProjectSaveService> SaveService { get; } = new();
		public Mock<IProjectHistoryService> HistoryService { get; } = new();

		public string Title => "TestEditorState";

		public IProjectLoadService GetLoadService() => LoadService.Object;

		public IProjectSaveService GetSaveService() => SaveService.Object;

		public IProjectHistoryService GetHistoryService() => HistoryService.Object;
	}
}
