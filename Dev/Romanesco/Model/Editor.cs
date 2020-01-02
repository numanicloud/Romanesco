using Romanesco.Annotations;
using Romanesco.Common.Entities;
using Romanesco.Model.EditorState;
using Romanesco.Model.Services.Serialize;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Romanesco.Model
{
    class Editor
    {
        private EditorState.EditorState editorState;
        private readonly ObjectInterpreter interpreter;

        public void Create()
        {
            editorState.GetLoadService().Create(interpreter);
            editorState.OnCreate();
        }

        public void Export()
        {
            editorState.GetSaveService().Export();
            editorState.OnExport();
        }

        public void Open()
        {
            editorState.GetLoadService().Open();
            editorState.OnOpen();
        }

        public async Task SaveAsync()
        {
            await editorState.GetSaveService().SaveAsync();
            editorState.OnSave();
        }

        public async Task SaveAsAsync()
        {
            await editorState.GetSaveService().SaveAsAsync();
            editorState.OnSaveAs();
        }

        public void Undo()
        {
            editorState.GetHistoryService().Undo();
            editorState.OnUndo();
        }

        public void Redo()
        {
            editorState.GetHistoryService().Redo();
            editorState.OnRedo();
        }

        public void ChangeState(EditorState.EditorState state)
        {
            editorState = state;
        }

        // 仮
        public ProjectComponents.Project LoadProject(Common.IStateFactory[] factories,
            Services.IProjectSettingProvider settingProvider)
        {
            var settings = settingProvider.GetSettings();
            var instance = Activator.CreateInstance(settings.ProjectType);

            var interpreter = new ObjectInterpreter(factories);
            var properties = settings.ProjectType.GetProperties()
                .Where(p => p.GetCustomAttribute<EditorMemberAttribute>() != null)
                .Select(p => interpreter.InterpretAsState(instance, p))
                .ToArray();
            var fields = settings.ProjectType.GetFields()
                .Where(f => f.GetCustomAttribute<EditorMemberAttribute>() != null)
                .Select(f => interpreter.InterpretAsState(instance, f))
                .ToArray();

            var stateRoot = new StateRoot()
            {
                States = properties.Concat(fields).ToArray(),
            };
            var project = new ProjectComponents.Project(settings, stateRoot);

            editorState = new NewEditorState(this, project);

            return project;
        }
    }
}
