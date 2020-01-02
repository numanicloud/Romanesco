using Romanesco.Annotations;
using Romanesco.Common.Entities;
using Romanesco.Model.EditorState;
using System;
using System.Linq;
using System.Reflection;

namespace Romanesco.Model
{
    class Editor
    {
        private EditorState.EditorState editorState;

        public void Create() => editorState.GetLoadService().Create();

        public void Export() => editorState.GetSaveService().Export();

        public void Open() => editorState.GetLoadService().Open();

        public void Save() => editorState.GetSaveService().Save();

        public void SaveAs() => editorState.GetSaveService().SaveAs();

        public void Undo() => editorState.GetHistoryService().Undo();

        public void Redo() => editorState.GetHistoryService().Redo();

        public void ChangeState(EditorState.EditorState state)
        {
            editorState = state;
        }

        // 仮
        public StateRoot LoadRoots(Common.IStateFactory[] factories, Type rootType)
        {
            var interpreter = new ObjectInterpreter(factories);
            var properties = rootType.GetProperties()
                .Where(p => p.GetCustomAttribute<EditorMemberAttribute>() != null)
                .Select(p => interpreter.InterpretAsState(p))
                .ToArray();
            var fields = rootType.GetFields()
                .Where(f => f.GetCustomAttribute<EditorMemberAttribute>() != null)
                .Select(f => interpreter.InterpretAsState(f))
                .ToArray();

            editorState = new DirtyEditorState(this);

            return new StateRoot()
            {
                States = properties.Concat(fields).ToArray(),
            };
        }
    }
}
