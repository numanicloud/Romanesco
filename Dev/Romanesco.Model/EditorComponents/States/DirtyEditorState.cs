using Romanesco.Model.ProjectComponents;
using Romanesco.Model.Services.History;
using Romanesco.Model.Services.Load;
using Romanesco.Model.Services.Save;
using Romanesco.Model.Services.Serialize;

namespace Romanesco.Model.EditorComponents.States
{
    class DirtyEditorState : EditorState
    {
        private readonly IProjectLoadService loadService;
        private readonly IProjectSaveService saveService;
        private readonly IProjectHistoryService historyService;

		public override string Title => $"Romanesco - {System.IO.Path.GetFileName(Context.CurrentProject.Project.DefaultSavePath)} (変更あり)";

        public DirtyEditorState(EditorContext context) : base(context)
        {
            var deserializer = new NewtonsoftStateDeserializer();
            var serializer = new NewtonsoftStateSerializer();
            loadService = new WindowsLoadService(context, deserializer);
            saveService = new WindowsSaveService(context.CurrentProject.Project, serializer);
            historyService = new SimpleHistoryService(context.CurrentProject);
		}

        public override IProjectHistoryService GetHistoryService() => historyService;

        public override IProjectLoadService GetLoadService() => loadService;

        public override IProjectSaveService GetSaveService() => saveService;

        public override void OnSave()
        {
            Context.Editor.ChangeState(new CleanEditorState(Context));
        }

        public override void OnSaveAs() => OnSave();
    }
}
