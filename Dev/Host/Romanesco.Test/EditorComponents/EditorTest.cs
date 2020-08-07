using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using Moq;
using Moq.Linq;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Implementations;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.EditorComponents;
using Romanesco.Model.EditorComponents.States;
using Romanesco.Model.Infrastructure;
using Romanesco.Model.Services.History;
using Romanesco.Model.Services.Load;
using Romanesco.Model.Services.Save;
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

		//[Fact]
		public void プロジェクトを作成する命令をエディターが現在のステートに割り振る()
		{
			// EditorState抽象クラスには引数無しコンストラクタがないのでインスタンス化できない
			// インターフェースにすべきだろう
			// IEditorStateChangerに状態の初期化を任せるのもやめて、EmptyStateを(IEditorStateとして)直接注入したい
			// Editorのコンストラクタに渡すとして、IEditorStateに対する注入がEmptyで固定されるのが気持ち悪い
			// IInitialEditorState みたいなインターフェースが必要？

			// Arrange
			var loadService = new Mock<IProjectLoadService>();
			loadService.Setup(x => x.CreateAsync())
				.Returns(async () => null);
			loadService.Setup(x => x.CanCreate).Returns(true);
			loadService.Setup(x => x.CanOpen).Returns(true);

			var editorState = new Mock<IEditorState>();
			editorState.Setup(x => x.GetLoadService()).Returns(loadService.Object);
			editorState.Setup(x => x.GetHistoryService()).Returns(new SimpleHistoryService(new CommandHistory()));
			editorState.Setup(x => x.GetSaveService()).Returns(new NullSaveService());

			var editorStateChanger = new Mock<IEditorStateChanger>();
			editorStateChanger.Setup(x => x.OnChange)
				.Returns(() => Observable.Never<EditorState>());

			var editor = new Editor(editorStateChanger.Object, editorState.Object);

			// Act
			var projectContext = editor.CreateAsync().Result;

			// Assert
			loadService.Verify(x => x.CreateAsync(), Times.Once);
		}

		//[Fact]
		public void プロジェクトを作成することができる()
		{
			// この機能だけでオブジェクトがたくさん必要だ
			// 複数のフェーズに分けたりできないものか？
			// 単にテストをフィクスチャに分ければよいというのはある

			// Editorがどのオブジェクトの何を呼ぶかに対してアサーションをかけるのがよい？
			// あるクラスに渡した別のクラスがどのようなメッセージを受け取るのかについて期待するのがモヤっとする
			// メソッドの内部の実装は隠蔽されるもので、一旦中へ入っていったインスタンスがどうなるのかは知るべきではないのでは？

			var expectedSavePath = "./HogePath/Project.roma";

			var editorStateChanger = new Mock<IEditorStateChanger>();

			var typeExporterMock = new Mock<IProjectTypeExporter>();
			var typeExporter = typeExporterMock.Object;

			var projectLoader = new Mock<IProjectLoadService>();
			projectLoader.Setup(x => x.CreateAsync())
				.Returns(async () =>
				{
					ProjectSettings settings = new ProjectSettings(GetType().Assembly, GetType(), GetType(), new string[0]);
					StateRoot root = new StateRoot(new object(), new IFieldState[]{new NoneState()});
					Project project = new Project(settings, root, new ProjectDependency[0])
					{
						DefaultSavePath = expectedSavePath
					};
					IProjectTypeExporter exporter = typeExporter;
					return new ProjectContext(project, exporter);
				});

			var projectModelFactory = new Mock<IProjectModelFactory>();
			projectModelFactory.Setup(x => x.ResolveNewEditorStateAsTransient())
				.Returns(new NewEditorState(projectLoader.Object, new NullHistoryService(), new NullSaveService(), projectModelFactory.Object, editorStateChanger.Object));

			var modelFactory = new Mock<IModelFactory>();
			modelFactory.Setup(x => x.ResolveProjectModelFactory(It.IsAny<ProjectContext>()))
				.Returns(projectModelFactory.Object);

			editorStateChanger.Setup(x => x.GetInitialState())
				.Returns(new EmptyEditorState(projectLoader.Object, modelFactory.Object, editorStateChanger.Object));
			editorStateChanger.Setup(x => x.OnChange)
				.Returns(new Subject<EditorState>());
			editorStateChanger.Setup(x => x.ChangeState(It.IsAny<EditorState>()))
				.Callback(() => { });

			var editor = new Editor(editorStateChanger.Object, new EmptyEditorState(projectLoader.Object, modelFactory.Object, editorStateChanger.Object));

			var projectContext = editor.CreateAsync().Result;

			if (AssertHelper.NotNull(projectContext))
			{
				Assert.Equal(expectedSavePath, projectContext.Project.DefaultSavePath);
				Assert.Same(typeExporter, projectContext.Exporter);
			}
		}
	}
}
