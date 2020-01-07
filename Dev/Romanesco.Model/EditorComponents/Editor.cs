using Reactive.Bindings;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.Model.ProjectComponents;
using Romanesco.Model.EditorComponents.States;
using Romanesco.Model.ProjectComponents;
using Romanesco.Model.Services;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace Romanesco.Model.EditorComponents
{
    class Editor : IEditorFacade
    {
        private EditorState editorState;
        private readonly EditorContext context;
        private readonly IStateFactoryProvider factoryProvider;
        private readonly Subject<(EditorCommandType command, bool canExecute)> canExecuteSubject;

        public EditorContext Context => context;
        public ReactiveProperty<string> ApplicationTitle { get; }
        public IObservable<(EditorCommandType command, bool canExecute)> CanExecuteObservable { get; }

        public Editor(IStateFactoryProvider factoryProvider, IProjectSettingProvider settingProvider)
        {
            this.factoryProvider = factoryProvider;
            context = new EditorContext(this, null, settingProvider);
            editorState = new EmptyEditorState(context);

            ApplicationTitle = new ReactiveProperty<string>(editorState.Title);
            canExecuteSubject = new Subject<(EditorCommandType command, bool canExecute)>();

            var initialCanExecute = new ReplaySubject<(EditorCommandType command, bool canExecute)>();
            UpdateCanExecute(initialCanExecute);

            CanExecuteObservable = initialCanExecute.Concat(canExecuteSubject);
        }

        public ProjectContext Create()
        {
            return ResetProject(() => Task.FromResult(editorState.GetLoadService().Create()),
                () =>
                {
                    editorState.OnCreate();
                    editorState = new NewEditorState(context);
                }).Result;
        }

        public Task<ProjectContext> OpenAsync()
        {
            return ResetProject(async () => await editorState.GetLoadService().OpenAsync(),
                () =>
                {
                    editorState.OnOpen();
                    editorState = new CleanEditorState(context);
                });
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

        public void Export()
        {
            editorState.GetSaveService().Export();
            editorState.OnExport();
        }

        public void Undo()
        {
            var history = editorState.GetHistoryService();
            history.Undo();
            editorState.OnUndo();
            canExecuteSubject.OnNext((EditorCommandType.Undo, history.CanUndo));
        }

        public void Redo()
        {
            var history = editorState.GetHistoryService();
            history.Redo();
            editorState.OnRedo();
            canExecuteSubject.OnNext((EditorCommandType.Redo, history.CanRedo));
        }

        public void ChangeState(EditorState state)
        {
            editorState = state;
            UpdateTitle();
            UpdateCanExecute(canExecuteSubject);
        }

        private ObjectInterpreter CreateInterpreter(ProjectContextCrawler context)
        {
            return new ObjectInterpreter(factoryProvider.GetStateFactories(context).ToArray());
        }

        private void UpdateTitle() => ApplicationTitle.Value = editorState.Title;

        private async Task<ProjectContext> ResetProject(Func<Task<Project>> generator, Action onSuccess)
        {
            var contextCrawler = new ProjectContextCrawler();
            context.Interpreter = CreateInterpreter(contextCrawler);

            var project = await generator();
            if (project != null)
            {
                context.CurrentProject = new ProjectContext(project, contextCrawler);
                onSuccess();
                UpdateTitle();
            }
            return context.CurrentProject;
        }

        private void UpdateCanExecute(IObserver<(EditorCommandType, bool)> observer)
        {
            var load = editorState.GetLoadService();
            var save = editorState.GetSaveService();
            var history = editorState.GetHistoryService();

            observer.OnNext((EditorCommandType.Create, load.CanCreate));
            observer.OnNext((EditorCommandType.Open, load.CanOpen));
            observer.OnNext((EditorCommandType.Save, save.CanSave));
            observer.OnNext((EditorCommandType.SaveAs, save.CanSave));
            observer.OnNext((EditorCommandType.Export, save.CanExport));
            observer.OnNext((EditorCommandType.Undo, history.CanUndo));
            observer.OnNext((EditorCommandType.Redo, history.CanRedo));
        }
    }
}
