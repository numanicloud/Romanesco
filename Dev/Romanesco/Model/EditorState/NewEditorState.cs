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

        public override string Title => "Romanesco - 新規プロジェクト";

        public NewEditorState(EditorContext context, Project project) : base(context)
        {
            var deserializer = new NewtonsoftStateDeserializer();
            var serializer = new NewtonsoftStateSerializer();
            loadService = new WindowsLoadService(context, deserializer);
            saveService = new WindowsSaveService(project, serializer);
            historyService = new NullHistoryService();
            this.project = project;
        }

        public override IProjectLoadService GetLoadService() => loadService;

        public override IProjectSaveService GetSaveService() => saveService;

        public override IProjectHistoryService GetHistoryService() => historyService;

        public override void OnSave()
        {
            Context.Editor.ChangeState(new CleanEditorState(Context, project));
        }

        public override void OnSaveAs() => OnSave();

        public override void OnEdit()
        {
            Context.Editor.ChangeState(new DirtyEditorState(Context, project));
        }
    }
}
