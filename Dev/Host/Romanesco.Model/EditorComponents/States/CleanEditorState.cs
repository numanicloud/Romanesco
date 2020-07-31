using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.Infrastructure;
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
		private readonly ProjectContext projectContext;

		public override string Title => $"Romanesco - {System.IO.Path.GetFileName(projectContext.Project.DefaultSavePath ?? "")}";

		public CleanEditorState(IProjectLoadService loadService,
			IProjectHistoryService historyService,
			IProjectSaveService saveService,
			ProjectContext projectContext,
			IProjectModelFactory factory,
			EditorStateChanger2 stateChanger)
			: base(factory, stateChanger)
		{
			this.projectContext = projectContext;
			this.loadService = loadService;
			this.saveService = saveService;
			this.historyService = historyService;
		}

		public override IProjectLoadService GetLoadService() => loadService;

		public override IProjectSaveService GetSaveService() => saveService;

		public override IProjectHistoryService GetHistoryService() => historyService;

		public override void OnSave()
		{
			// この状態自身へは遷移しない
		}

		public override void OnSaveAs()
		{
			// この状態自身へは遷移しない
		}
	}
}
