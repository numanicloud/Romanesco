using Reactive.Bindings;
using Romanesco.Common.Model.Basics;
using Romanesco.Model.EditorComponents.States;
using Romanesco.Model.Services.Load;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows.Documents;
using Reactive.Bindings.Extensions;

namespace Romanesco.Model.EditorComponents
{
	internal sealed class Editor : IEditorFacade, IDisposable
    {
        private EditorState editorState;
        public List<IDisposable> Disposables { get; } = new List<IDisposable>();

        private readonly ReplaySubject<(EditorCommandType command, bool canExecute)> canExecuteSubject
            = new ReplaySubject<(EditorCommandType command, bool canExecute)>();

        public ReactiveProperty<string> ApplicationTitle { get; } = new ReactiveProperty<string>();
        public IObservable<(EditorCommandType command, bool canExecute)> CanExecuteObservable
            => canExecuteSubject;

		public Editor(EditorStateChanger stateChanger)
		{
            stateChanger.OnChange.Subscribe(x => ChangeState(x))
	            .AddTo(Disposables);
            stateChanger.InitializeState(ref editorState);
        }

		public async Task<ProjectContext?> CreateAsync()
        {
            var projectContext = await ResetProject(x => x.CreateAsync());
            if (projectContext is { } c)
			{
                editorState.OnCreate(projectContext);
			}
            return projectContext;
        }

        public async Task<ProjectContext?> OpenAsync()
        {
            var projectContext = await ResetProject(x => x.OpenAsync());
            if (projectContext is { } c)
            {
                editorState.OnOpen(projectContext);
            }
            return projectContext;
        }

        private async Task<ProjectContext?> ResetProject(Func<IProjectLoadService, Task<ProjectContext?>> generator)
        {
            var projectContext = await generator(editorState.GetLoadService());
            if (projectContext is null)
            {
                return null;
            }

            UpdateTitle();

            Observable.Merge(projectContext.Project.Root.States.Select(x => x.OnEdited))
                .Subscribe(x => OnEdit())
                .AddTo(Disposables);

            return projectContext;
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

        public void Dispose()
        {
	        canExecuteSubject.Dispose();
	        ApplicationTitle.Dispose();
	        foreach (var disposable in Disposables)
	        {
		        disposable.Dispose();
	        }
        }
    }
}
