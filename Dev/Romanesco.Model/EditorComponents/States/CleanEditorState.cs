using Romanesco.Common.Model.Interfaces;
using Romanesco.Model.ProjectComponents;
using Romanesco.Model.Services.History;
using Romanesco.Model.Services.Load;
using Romanesco.Model.Services.Save;
using Romanesco.Model.Services.Serialize;

namespace Romanesco.Model.EditorComponents.States
{
    class CleanEditorState : EditorState
    {
        private readonly IProjectLoadService loadService;
        private readonly IProjectSaveService saveService;
        private readonly IProjectHistoryService historyService;
        private readonly IStateFactoryProvider stateFactoryProvider;

        public override string Title => $"Romanesco - {System.IO.Path.GetFileName(Context.CurrentProject?.Project.DefaultSavePath ?? "")}";

        public EditorContext Context { get; }

        public CleanEditorState(EditorContext context, IStateFactoryProvider stateFactoryProvider)
        {
            var deserializer = new NewtonsoftStateDeserializer();
            var serializer = new NewtonsoftStateSerializer();
            loadService = new WindowsLoadService(context.SettingProvider, stateFactoryProvider, deserializer);
            saveService = new WindowsSaveService(context.CurrentProject.Project, serializer, context.CurrentProject.Exporter);
            historyService = new SimpleHistoryService(context.CurrentProject);
            Context = context;
            this.stateFactoryProvider = stateFactoryProvider;
        }

        public override IProjectLoadService GetLoadService() => loadService;

        public override IProjectSaveService GetSaveService() => saveService;

        public override IProjectHistoryService GetHistoryService() => historyService;

        public override void OnEdit()
        {
            Context.Editor.ChangeState(new DirtyEditorState(Context, stateFactoryProvider));
        }
    }
}
