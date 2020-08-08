using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.Commands;
using Romanesco.Model.Infrastructure;
using Romanesco.Model.Services.History;
using Romanesco.Model.Services.Load;
using Romanesco.Model.Services.Save;

namespace Romanesco.Model.EditorComponents.States
{
	internal class DirtyEditorState : ProjectSpecifiedEditorState
	{
		private readonly IProjectLoadService loadService;
		private readonly IProjectSaveService saveService;
		private readonly IProjectHistoryService historyService;
		private readonly IProjectContext projectContext;

		public override string Title => $"Romanesco - {System.IO.Path.GetFileName(projectContext.Project.DefaultSavePath)} (変更あり)";

		public DirtyEditorState(IProjectLoadService loadService,
			IProjectHistoryService historyService,
			IProjectSaveService saveService,
			IProjectContext project,
			IProjectModelFactory factory,
			EditorSession editorSession)
			: base(factory, editorSession)
		{
			this.projectContext = project;
			this.saveService = saveService;
			this.loadService = loadService;
			this.historyService = historyService;
		}

		public override IProjectHistoryService GetHistoryService() => historyService;

		public override IProjectLoadService GetLoadService() => loadService;

		public override IProjectSaveService GetSaveService() => saveService;

		public override void OnEdit()
		{
			// この状態自身へは遷移しない
		}
	}
}
