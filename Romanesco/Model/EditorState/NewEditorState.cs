using Romanesco.Common.Entities;
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

        public override void Create()
        {
        }

        public override void Export()
        {
        }

        public override void OnEdit()
        {
        }

        public override void Open()
        {
        }

        public override void Redo()
        {
        }

        public override void Save()
        {
        }

        public override void Undo()
        {
        }
    }
}
