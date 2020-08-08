using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.Services.Load;

namespace Romanesco.Test.Helpers
{
	static class MockHelper
	{
		public static Mock<IProjectLoadService> GetLoadMock(IProjectContext? projectContext = null,
			bool? canCreate = null,
			bool? canOpen = null)
		{
			var mock = new Mock<IProjectLoadService>();

			mock.Setup(x => x.CreateAsync())
				.Returns(async () => projectContext);

			if (canCreate.HasValue)
			{
				mock.Setup(x => x.CanCreate)
					.Returns(canCreate.Value);
			}

			if (canOpen != null)
			{
				mock.Setup(x => x.CanOpen)
					.Returns(canOpen.Value);
			}

			return mock;
		}
	}
}
