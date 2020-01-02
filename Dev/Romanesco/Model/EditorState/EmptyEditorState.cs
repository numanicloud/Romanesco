using Romanesco.Model.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Model.EditorState
{
    class EmptyEditorState : EditorState
    {
        public override bool CanSave => false;

        public EmptyEditorState(Editor editor) : base(editor)
        {
        }

        public override IProjectLoadService GetLoadService() => new NullLoadService();

        public override IProjectSaveService GetSaveService() => new NullSaveService();

        public override IProjectHistoryService GetHistoryService() => new NullHistoryService();
    }
}
