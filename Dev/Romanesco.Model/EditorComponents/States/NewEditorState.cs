using Romanesco.Common.Model.Interfaces;
using Romanesco.Model.Services.History;
using Romanesco.Model.Services.Load;
using Romanesco.Model.Services.Save;

namespace Romanesco.Model.EditorComponents.States
{
	class NewEditorState : EditorState
    {
        private readonly IProjectLoadService loadService;
        private readonly IProjectSaveService saveService;
        private readonly IProjectHistoryService historyService;
		private readonly IServiceLocator serviceLocator;

        public override string Title => "Romanesco - 新規プロジェクト";

        private EditorContext Context { get; }

        public NewEditorState(IProjectLoadService loadService,
            IProjectHistoryService historyService,
            IServiceLocator serviceLocator,
            EditorContext context)
        {
			this.loadService = loadService;
            this.saveService = serviceLocator.CreateInstance<IProjectSaveService>(context.CurrentProject);
            this.historyService = historyService;
			this.serviceLocator = serviceLocator;
            Context = context;
		}

        public override IProjectLoadService GetLoadService() => loadService;

        public override IProjectSaveService GetSaveService() => saveService;

        public override IProjectHistoryService GetHistoryService() => historyService;

        public override void OnSave()
        {
            // TODO: ContextをDIコンテナに任せられないものか？
            Context.Editor.ChangeState(serviceLocator.CreateInstance<CleanEditorState>(Context));
        }

        public override void OnSaveAs() => OnSave();

        public override void OnEdit()
        {
            Context.Editor.ChangeState(serviceLocator.CreateInstance<DirtyEditorState>(Context));
        }
    }
}
