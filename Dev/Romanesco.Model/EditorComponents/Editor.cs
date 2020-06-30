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
using System.Reflection;
using System.Threading.Tasks;

namespace Romanesco.Model.EditorComponents
{
    class Editor : IEditorFacade
    {
        private EditorState editorState;
        private readonly IStateFactoryProvider factoryProvider;
        private readonly IProjectSettingProvider settingProvider;
        private readonly Subject<(EditorCommandType command, bool canExecute)> canExecuteSubject;

        public ReactiveProperty<string> ApplicationTitle { get; }
        public IObservable<(EditorCommandType command, bool canExecute)> CanExecuteObservable { get; }

        public Editor(IStateFactoryProvider factoryProvider, IProjectSettingProvider settingProvider)
        {
            this.factoryProvider = factoryProvider;
            this.settingProvider = settingProvider;
            editorState = new EmptyEditorState(settingProvider, factoryProvider);

            ApplicationTitle = new ReactiveProperty<string>(editorState.Title);
            canExecuteSubject = new Subject<(EditorCommandType command, bool canExecute)>();

            var initialCanExecute = new ReplaySubject<(EditorCommandType command, bool canExecute)>();
            UpdateCanExecute(initialCanExecute);
            initialCanExecute.OnCompleted();

            CanExecuteObservable = initialCanExecute.Concat(canExecuteSubject);
        }

        public Task<ProjectContext?> CreateAsync()
        {
            return ResetProject(async () => await editorState.GetLoadService().CreateAsync(),
                context =>
                {
                    editorState.OnCreate();
                    ChangeState(new NewEditorState(context, factoryProvider));
                });
        }

        public Task<ProjectContext?> OpenAsync()
        {
            return ResetProject(async () => await editorState.GetLoadService().OpenAsync(),
                context =>
                {
                    editorState.OnOpen();
                    ChangeState(new CleanEditorState(context, factoryProvider));
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

        public async Task ExportAsync()
        {
            await editorState.GetSaveService().ExportAsync();
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

        private void OnEdit()
        {
            editorState.OnEdit();
            var history = editorState.GetHistoryService();
            canExecuteSubject.OnNext((EditorCommandType.Undo, history.CanUndo));
            canExecuteSubject.OnNext((EditorCommandType.Redo, history.CanRedo));
        }

        private void UpdateTitle() => ApplicationTitle.Value = editorState.Title;

        private async Task<ProjectContext?> ResetProject(Func<Task<ProjectContext?>> generator, Action<EditorContext> onSuccess)
        {
            var projectContext = await generator();
            if (projectContext is null)
            {
                return null;
            }

            var editorContext = new EditorContext(this, settingProvider, projectContext);
            onSuccess(editorContext);
            UpdateTitle();

            Observable.Merge(projectContext.Project.Root.States.Select(x => x.OnEdited))
                .Subscribe(x => OnEdit());

            return projectContext;
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
