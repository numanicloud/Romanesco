using Romanesco.Model.ProjectComponents;
using Romanesco.Model.Services;
using Romanesco.Model.Services.Save;
using Romanesco.Model.Services.Serialize;

namespace Romanesco.Model.EditorState
{
    class NewEditorState : EditorState
    {
        private readonly IProjectLoadService loadService;
        private readonly IProjectSaveService saveService;
        private readonly IProjectHistoryService historyService;
        private readonly Project project;

        public override bool CanSave => true;

        public NewEditorState(Editor editor, Project project) : base(editor)
        {
            loadService = new NullLoadService();
            saveService = new WindowsSaveService(project, null, new NewtonsoftStateSerializer());
            historyService = new NullHistoryService();
            this.project = project;
        }

        public override IProjectLoadService GetLoadService() => loadService;

        public override IProjectSaveService GetSaveService() => saveService;

        public override IProjectHistoryService GetHistoryService() => historyService;

        public override void OnSave()
        {
            Editor.ChangeState(new CleanEditorState(Editor, project));
        }

        public override void OnSaveAs() => OnSave();
    }
}
