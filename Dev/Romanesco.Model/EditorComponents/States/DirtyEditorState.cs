using Romanesco.Common.Model.Interfaces;
using Romanesco.Model.Services.History;
using Romanesco.Model.Services.Load;
using Romanesco.Model.Services.Save;

namespace Romanesco.Model.EditorComponents.States
{
	class DirtyEditorState : EditorState
    {
        private readonly IProjectLoadService loadService;
        private readonly IProjectSaveService saveService;
        private readonly IProjectHistoryService historyService;
		private readonly IServiceLocator serviceLocator;

        public override string Title => $"Romanesco - {System.IO.Path.GetFileName(Context.CurrentProject.Project.DefaultSavePath)} (変更あり)";

        public EditorContext Context { get; }

        public DirtyEditorState(IProjectLoadService loadService,
            IProjectHistoryService historyService,
            IServiceLocator serviceLocator,
            EditorContext context)
        {
            Context = context;
			this.loadService = loadService;
            this.saveService = serviceLocator.CreateInstance<IProjectSaveService>(context.CurrentProject);
            this.historyService = historyService;
			this.serviceLocator = serviceLocator;
		}

        public override IProjectHistoryService GetHistoryService() => historyService;

        public override IProjectLoadService GetLoadService() => loadService;

        public override IProjectSaveService GetSaveService() => saveService;

        public override void OnSave()
        {
            Context.Editor.ChangeState(serviceLocator.CreateInstance<CleanEditorState>(Context));
        }

        public override void OnSaveAs() => OnSave();
    }
}
