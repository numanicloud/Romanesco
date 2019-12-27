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

        public override void Create()
        {
            Editor.ChangeState(new NewEditorState(Editor, CreateService.Create()));
        }

        public override void Export()
        {
            // do nothing
        }

        public override void Open()
        {
        }

        public override void Save()
        {
            // do nothing
        }

        public override void OnEdit()
        {
            // do nothing
        }

        public override void Undo()
        {
            // do nothing
        }

        public override void Redo()
        {
            // do nothing
        }
    }
}
