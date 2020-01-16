using Romanesco.Model.ProjectComponents;
using Romanesco.Model.Services.History;
using Romanesco.Model.Services.Load;
using Romanesco.Model.Services.Save;
using Romanesco.Model.Services.Serialize;

namespace Romanesco.Model.EditorComponents.States
{
    class NewEditorState : EditorState
    {
        private readonly IProjectLoadService loadService;
        private readonly IProjectSaveService saveService;
        private readonly IProjectHistoryService historyService;

        public override string Title => "Romanesco - 新規プロジェクト";

        public NewEditorState(EditorContext context) : base(context)
        {
            var deserializer = new NewtonsoftStateDeserializer();
            var serializer = new NewtonsoftStateSerializer();
            loadService = new WindowsLoadService(context, deserializer);
            saveService = new WindowsSaveService(context.CurrentProject.Project, serializer, context.CurrentProject.Exporter);
            historyService = new SimpleHistoryService(context.CurrentProject);
        }

        public override IProjectLoadService GetLoadService() => loadService;

        public override IProjectSaveService GetSaveService() => saveService;

        public override IProjectHistoryService GetHistoryService() => historyService;

        public override void OnSave()
        {
            Context.Editor.ChangeState(new CleanEditorState(Context));
        }

        public override void OnSaveAs() => OnSave();

        public override void OnEdit()
        {
            Context.Editor.ChangeState(new DirtyEditorState(Context));
        }
    }
}
