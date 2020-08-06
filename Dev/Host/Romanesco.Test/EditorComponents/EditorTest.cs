using System;
using System.Collections.Generic;
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
		public void プロジェクトを作成する命令をエディターが現在のステートに割り振る()
		{
			// EditorState抽象クラスには引数無しコンストラクタがないのでインスタンス化できない
			// インターフェースにすべきだろう
			// IEditorStateChangerに状態の初期化を任せるのもやめて、EmptyStateを(IEditorStateとして)直接注入したい
			// Editorのコンストラクタに渡すとして、IEditorStateに対する注入がEmptyで固定されるのが気持ち悪い
			// IInitialEditorState みたいなインターフェースが必要？

			var editorState = new Mock<EditorState>();
			editorState.Setup(x => x.CreateAsync())
				.Returns(async () => (ProjectContext?) null);

			var editorStateChanger = new Mock<IEditorStateChanger>();
			editorStateChanger.Setup(x => x.GetInitialState())
				.Returns(editorState.Object);

			var editor = new Editor(editorStateChanger.Object);

			var projectContext = editor.CreateAsync().Result;
			editorState.Verify(x => x.CreateAsync(), Times.Once);
		}

		[Fact]
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

			var editor = new Editor(editorStateChanger.Object);

			var projectContext = editor.CreateAsync().Result;

			if (AssertHelper.NotNull(projectContext))
			{
				Assert.Equal(expectedSavePath, projectContext.Project.DefaultSavePath);
				Assert.Same(typeExporter, projectContext.Exporter);
			}
		}
	}
}
