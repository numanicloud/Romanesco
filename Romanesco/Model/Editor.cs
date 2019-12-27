using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Model
{
    class Editor
    {
        private EditorState.EditorState editorState;

        public void Create()
        {
        }

        public void Export()
        {
        }

        public void Open()
        {
        }

        public void Save()
        {
        }

        public void Undo()
        {
        }

        public void Redo()
        {
        }

        public void ChangeState(EditorState.EditorState state)
        {
            editorState = state;
        }
    }
}
