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

		[Fact]
		public void ステートが変わるとCanSaveが更新される()
		{
			var saverService1 = MockHelper.GetSaveServiceMock(canSave: false, canExport: false);
			var saverService2 = MockHelper.GetSaveServiceMock(canSave: true, canExport: true);
			var currentState = MockHelper.GetEditorStateMock(saveService: saverService1.Object);
			var nextState = MockHelper.GetEditorStateMock(saveService: saverService2.Object);

			var commandRouter = new CommandRouter(currentState.Object, Mock.Of<IEditorStateRepository>());

			Assert.False(commandRouter.CanSave.Value);
			commandRouter.UpdateState(nextState.Object);
			Assert.True(commandRouter.CanSave.Value);
		}
	}
}
