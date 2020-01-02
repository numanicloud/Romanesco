using Romanesco.Common.Entities;
using Romanesco.Model.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Model.EditorState
{
    class NewEditorState : EditorState
    {
        private readonly Project project;

        public override bool CanSave => true;

        public NewEditorState(Editor editor, Project project) : base(editor)
        {
            this.project = project;
        }

        public override IProjectLoadService GetLoadService() => new NullLoadService();

        public override IProjectSaveService GetSaveService() => new NullSaveService();

        public override IProjectHistoryService GetHistoryService() => new NullHistoryService();
    }
}
