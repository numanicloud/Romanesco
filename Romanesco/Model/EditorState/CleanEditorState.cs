using Romanesco.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Model.EditorState
{
    class CleanEditorState : EditorState
    {
        private readonly Project project;

        public override bool CanSave => true;

        public CleanEditorState(Editor editor, Project project) : base(editor)
        {
            this.project = project;
        }

        public override void Create()
        {
            Editor.ChangeState(new NewEditorState(Editor, CreateService.Create()));
        }

        public override void Export()
        {
            ExportService.Export(project);
        }

        public override void Open()
        {
            Editor.ChangeState(new CleanEditorState(Editor, OpenService.Open()));
        }

        public override void Save()
        {
            SaveService.Save(project);
        }

        public override void OnEdit()
        {
            throw new NotImplementedException();
        }

        public override void Redo()
        {
            throw new NotImplementedException();
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
