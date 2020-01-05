using Romanesco.Model.ProjectComponents;
using Romanesco.Model.Services;
using Romanesco.Model.Services.Save;
using Romanesco.Model.Services.Serialize;
using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Model.EditorState
{
    class DirtyEditorState : EditorState
    {
        private readonly IProjectLoadService loadService;
        private readonly IProjectSaveService saveService;
        private readonly IProjectHistoryService historyService;
        private readonly Project project;

        public override string Title => $"Romanesco - {System.IO.Path.GetFileName(project.DefaultSavePath)} (変更あり)";

        public DirtyEditorState(EditorContext context, Project project) : base(context)
        {
            var deserializer = new NewtonsoftStateDeserializer();
            var serializer = new NewtonsoftStateSerializer();
            loadService = new WindowsLoadService(context, deserializer);
            saveService = new WindowsSaveService(project, serializer);
            historyService = new NullHistoryService();
            this.project = project;
        }

        public override IProjectHistoryService GetHistoryService() => historyService;

        public override IProjectLoadService GetLoadService() => loadService;

        public override IProjectSaveService GetSaveService() => saveService;

        public override void OnSave()
        {
            Context.Editor.ChangeState(new CleanEditorState(Context, project));
        }

        public override void OnSaveAs() => OnSave();
    }
}
