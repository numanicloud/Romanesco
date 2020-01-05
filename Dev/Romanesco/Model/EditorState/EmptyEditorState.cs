using Romanesco.Model.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Model.EditorState
{
    class EmptyEditorState : EditorState
    {
        private readonly IProjectLoadService loadService;

        public override string Title => "Romanesco - プロジェクトなし";

        public EmptyEditorState(EditorContext context) : base(context)
        {
            var deserializer = new Services.Serialize.NewtonsoftStateDeserializer();
            loadService = new WindowsLoadService(context, deserializer);
        }

        public override IProjectLoadService GetLoadService() => loadService;

        public override IProjectSaveService GetSaveService() => new NullSaveService();

        public override IProjectHistoryService GetHistoryService() => new NullHistoryService();
    }
}
