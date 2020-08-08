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

namespace Romanesco.Test.Helpers
{
	static class MockHelper
	{
		public static Mock<IProjectLoadService> GetLoaderServiceMock(IProjectContext? projectContext = null,
			bool? canCreate = null,
			bool? canOpen = null)
		{
			var mock = new Mock<IProjectLoadService>();

			mock.Setup(x => x.CreateAsync())
				.Returns(async () => projectContext);

			mock.Setup(x => x.OpenAsync())
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
		
		public static Mock<IEditorState> GetEditorStateMock(CommandAvailability? commandAvailability = null,
			IProjectLoadService? loadService = null,
			IProjectSaveService? saveService = null,
			IProjectHistoryService? historyService = null)
		{
			var mock = new Mock<IEditorState>();
			
			mock.Setup(x => x.NotifyEdit())
				.Callback(() => { });

			if (loadService is {})
			{
				mock.Setup(x => x.GetLoadService())
					.Returns(loadService);
			}

			if (saveService is { })
			{
				mock.Setup(x => x.GetSaveService())
					.Returns(saveService);
			}

			if (historyService is { })
			{
				mock.Setup(x => x.GetHistoryService())
					.Returns(historyService);
			}

			return mock;
		}
		
		public static Mock<IProjectSaveService> GetSaveServiceMock(bool? canSave = null, bool? canExport = null)
		{
			var saveService = new Mock<IProjectSaveService>();
			saveService.Setup(x => x.SaveAsync())
				.Callback(async () => { });
			saveService.Setup(x => x.SaveAsAsync())
				.Callback(async () => { });
			saveService.Setup(x => x.ExportAsync())
				.Callback(async () => { });

			if (canSave.HasValue)
			{
				saveService.Setup(x => x.CanSave)
					.Returns(canSave.Value);
			}

			if (canExport.HasValue)
			{
				saveService.Setup(x => x.CanExport)
					.Returns(canExport.Value);
			}

			return saveService;
		}
	}
}
