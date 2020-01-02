using Romanesco.Model.Services;
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

        public DirtyEditorState(Editor editor) : base(editor)
        {
            loadService = new NullLoadService();
            saveService = new NullSaveService();
            historyService = new NullHistoryService();
        }

        public override bool CanSave => true;

        public override IProjectHistoryService GetHistoryService() => historyService;

        public override IProjectLoadService GetLoadService() => loadService;

        public override IProjectSaveService GetSaveService() => saveService;
    }
}
