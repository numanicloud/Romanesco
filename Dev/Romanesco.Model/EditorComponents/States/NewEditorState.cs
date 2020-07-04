using Romanesco.Common.Model.Basics;
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

        public override string Title => "Romanesco - 新規プロジェクト";

        public NewEditorState(IProjectLoadService loadService,
            IProjectHistoryService historyService,
            IProjectSaveService saveService,
            EditorStateChanger stateChanger)
            : base(stateChanger)
        {
			this.loadService = loadService;
            this.saveService = saveService;
            this.historyService = historyService;
		}

        public override IProjectLoadService GetLoadService() => loadService;

        public override IProjectSaveService GetSaveService() => saveService;

        public override IProjectHistoryService GetHistoryService() => historyService;
    }
}
