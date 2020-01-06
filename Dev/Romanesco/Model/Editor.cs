using Reactive.Bindings;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Model.EditorState;
using Romanesco.Model.ProjectComponents;
using Romanesco.Model.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Romanesco.Model
{
    class Editor
    {
        private EditorState.EditorState editorState;
        private readonly EditorContext context;
        private readonly IStateFactoryProvider factoryProvider;

        public ReactiveProperty<string> ApplicationTitle { get; }

        public Editor(IStateFactoryProvider factoryProvider, IProjectSettingProvider settingProvider)
        {
            this.factoryProvider = factoryProvider;
            context = new EditorContext(this, CreateInterpreter(), settingProvider);
            editorState = new EmptyEditorState(context);

            ApplicationTitle = new ReactiveProperty<string>(editorState.Title);
        }

        private ObjectInterpreter CreateInterpreter()
        {
            return new ObjectInterpreter(factoryProvider.GetStateFactories().ToArray());
        }

        private void UpdateTitle() => ApplicationTitle.Value = editorState.Title;

        public Project Create()
        {
            context.Interpreter = CreateInterpreter();
            var project = editorState.GetLoadService().Create();
            if (project != null)
            {
                editorState.OnCreate();
                editorState = new NewEditorState(context, project);
                UpdateTitle();
            }
            return project;
        }

        public void Export()
        {
            editorState.GetSaveService().Export();
            editorState.OnExport();
        }

        public async Task<Project> OpenAsync()
        {
            context.Interpreter = CreateInterpreter();
            var project = await editorState.GetLoadService().OpenAsync();
            if (project != null)
            {
                editorState.OnOpen();
                editorState = new CleanEditorState(context, project);
                UpdateTitle();
            }
            return project;
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
            UpdateTitle();
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
            UpdateTitle();
        }
    }
}
