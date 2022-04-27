using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.Commands;
using Romanesco.Model.Services.History;
using Romanesco.Model.Services.Load;
using Romanesco.Model.Services.Save;

namespace Romanesco.Model.EditorComponents.States
{
	internal class CleanEditorState : ProjectSpecifiedEditorState
	{
		private readonly IProjectLoadService loadService;
		private readonly IProjectSaveService saveService;
		private readonly IProjectHistoryService historyService;
		private readonly IProjectContext projectContext;

		public override string Title => $"Romanesco - {System.IO.Path.GetFileName(projectContext.Project.DefaultSavePath ?? "")}";

		public CleanEditorState(IProjectLoadService loadService,
			IProjectHistoryService historyService,
			IProjectSaveService saveService,
			IProjectContext projectContext)
		{
			this.projectContext = projectContext;
			this.loadService = loadService;
			this.saveService = saveService;
			this.historyService = historyService;
		}

		public override IProjectLoadService GetLoadService() => loadService;

		public override IProjectSaveService GetSaveService() => saveService;

		public override IProjectHistoryService GetHistoryService() => historyService;
	}
}
