using System;
using System.Reactive.Linq;
using Moq;
using Romanesco.Model.EditorComponents;
using Romanesco.Model.EditorComponents.States;
using Romanesco.Model.Services.Load;
using Xunit;

namespace Romanesco.Test.EditorComponents
{
	public class EditorTest
	{
		[Fact]
		public void プロジェクトを作成する命令をエディターが現在のステートに割り振る2()
		{
			// Arrange
			var loadService = new Mock<IProjectLoadService>();
			loadService.Setup(x => x.CreateAsync())
				.Returns(async () => null);

			var editorState = new Mock<IEditorState>();
			editorState.Setup(x => x.GetLoadService()).Returns(loadService.Object);
			editorState.Setup(x => x.UpdateCanExecute(It.IsAny<IObserver<(EditorCommandType, bool)>>()))
				.Callback(() => { });

			var editorStateChanger = new Mock<IEditorStateChanger>();
			editorStateChanger.Setup(x => x.OnChange)
				.Returns(() => Observable.Never<EditorState>());

			var editor = new Editor(editorStateChanger.Object, editorState.Object);

			// Act
			var projectContext = editor.CreateAsync().Result;

			// Assert
			loadService.Verify(x => x.CreateAsync(), Times.Once);
		}
	}
}
