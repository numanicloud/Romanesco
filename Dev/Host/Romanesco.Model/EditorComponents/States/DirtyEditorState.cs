using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.Services.History;
using Romanesco.Model.Services.Load;
using Romanesco.Model.Services.Save;

namespace Romanesco.Model.EditorComponents.States
{
	internal class DirtyEditorState : EditorState
    {
        private readonly IProjectLoadService loadService;
        private readonly IProjectSaveService saveService;
        private readonly IProjectHistoryService historyService;
		private readonly ProjectContext projectContext;

		public override string Title => $"Romanesco - {System.IO.Path.GetFileName(projectContext.Project.DefaultSavePath)} (変更あり)";

        public DirtyEditorState(IProjectLoadService loadService,
            IProjectHistoryService historyService,
            IProjectSaveService saveService,
            ProjectContext project,
            EditorStateChanger stateChanger)
            : base(stateChanger)
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
