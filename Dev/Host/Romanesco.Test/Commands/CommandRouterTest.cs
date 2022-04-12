using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.Commands;
using Romanesco.Model.EditorComponents;
using Romanesco.Model.EditorComponents.States;
using Romanesco.Model.Services.Load;
using Romanesco.Test.Helpers;
using Xunit;

namespace Romanesco.Test.Commands
{
	public class CommandRouterTest
	{
		[Fact]
		public void プロジェクトを作成した際に新しいステートへのメッセージが届く()
		{
			// IProjectContextから直接StateRootが取れないとテストが大変だ
			var project = new Mock<IProjectContext>();
			project.Setup(x => x.StateRoot)
				.Returns(() => new StateRoot(new object(), new IFieldState[0]));

			Mock<IProjectLoadService> loadService = new Mock<IProjectLoadService>();
			loadService.Setup(x => x.CreateAsync())
				.Returns(async () => project.Object);
			
			Mock<IEditorState> GetMockEditorState(string name)
			{
				var mock = MockHelper.GetEditorStateMock(loadService: loadService.Object);
				mock.Setup(x => x.OnCreate(It.IsAny<IProjectContext>()))
					.Callback(() => { });
				mock.Name = name;
				return mock;
			}

			var currentState = GetMockEditorState("Current");
			var nextState = GetMockEditorState("Next");
			var commandRouter = new CommandRouter(currentState.Object, Mock.Of<IEditorStateRepository>());
			commandRouter.OnCreate.Subscribe(x => commandRouter.UpdateState(nextState.Object));

			_ = commandRouter.CreateAsync().Result;
			_ = commandRouter.CreateAsync().Result;

			currentState.Verify(x => x.OnCreate(It.IsAny<IProjectContext>()), Times.Once);
			nextState.Verify(x => x.OnCreate(It.IsAny<IProjectContext>()), Times.Once);
		}

		[Fact]
		public void ステートが変わるとCanCreateが更新される()
		{
			var loadService1 = MockHelper.GetLoaderServiceMock(canCreate: true, canOpen: true);
			var loadService2 = MockHelper.GetLoaderServiceMock(canCreate: false, canOpen: false);
			var currentState = MockHelper.GetEditorStateMock(loadService: loadService1.Object);
			var nextState = MockHelper.GetEditorStateMock(loadService: loadService2.Object);

			var commandRouter = new CommandRouter(currentState.Object, Mock.Of<IEditorStateRepository>());

			Assert.True(commandRouter.CanCreate.Value);
			commandRouter.UpdateState(nextState.Object);
			Assert.False(commandRouter.CanCreate.Value);
		}
	}
}
