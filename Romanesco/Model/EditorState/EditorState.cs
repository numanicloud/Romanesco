using Romanesco.Model.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Model.EditorState
{
    abstract class EditorState
    {
        protected IProjectCreateService CreateService { get; }
        protected IProjectOpenService OpenService { get; }
        protected IProjectExportService ExportService { get; }
        protected IProjectSaveService SaveService { get; }
        protected Editor Editor { get; }

        public abstract bool CanSave { get; }

        public EditorState(Editor editor)
        {
            Editor = editor;
        }

        public abstract void Create();
        public abstract void Open();
        public abstract void Save();
        public abstract void SaveAs();
        public abstract void Export();
        public abstract void OnEdit();
        public abstract void Undo();
        public abstract void Redo();
    }
}
