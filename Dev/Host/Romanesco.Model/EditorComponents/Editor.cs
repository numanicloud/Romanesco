using Reactive.Bindings;
using Romanesco.Model.EditorComponents.States;
using Romanesco.Model.Services.Load;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Reactive.Bindings.Extensions;
using Romanesco.Common.Model.ProjectComponent;

namespace Romanesco.Model.EditorComponents
{
	internal sealed class Editor : IEditorFacade, IDisposable
    {
        private IEditorState editorState;
        public List<IDisposable> Disposables { get; } = new List<IDisposable>();

        private readonly ReplaySubject<(EditorCommandType command, bool canExecute)> canExecuteSubject
            = new ReplaySubject<(EditorCommandType command, bool canExecute)>();

        public ReactiveProperty<string> ApplicationTitle { get; } = new ReactiveProperty<string>();
        public IObservable<(EditorCommandType command, bool canExecute)> CanExecuteObservable
            => canExecuteSubject;

		public Editor(IEditorStateChanger stateChanger, IEditorState initialState)
		{
            stateChanger.OnChange.Subscribe(ChangeState).AddTo(Disposables);

            editorState = initialState;
            ChangeState(editorState);
        }

		public async Task<ProjectContext?> CreateAsync()
		{
			// ResetProjectメソッドの中では、結局EditorStateが持っているLoadServiceにメッセージを投げてる
			// EditorState自体にCreate/Loadのメソッドを用意すればよいだけでは？

			if (!(await editorState.GetLoadService().CreateAsync() is { } projectContext))
			{
				return null;
			}

			UpdateTitle();
			ObserveEdit(projectContext);
			editorState.OnCreate(projectContext);

			return projectContext;
		}

		private void ObserveEdit(ProjectContext projectContext)
		{
			projectContext.Project.Root.States
				.Select(x => x.OnEdited)
				.Merge()
				.Subscribe(x => OnEdit())
				.AddTo(Disposables);
		}

		public async Task<ProjectContext?> OpenAsync()
        {
	        Func<IProjectLoadService, Task<ProjectContext?>> generator = x => x.OpenAsync();
            
	        var projectContext = await editorState.GetLoadService().OpenAsync();
	        if (projectContext is null)
	        {
		        return null;
	        }

	        UpdateTitle();

	        projectContext.Project.Root.States
		        .Select(x => x.OnEdited)
		        .Merge()
		        .Subscribe(x => OnEdit())
		        .AddTo(Disposables);

            if (projectContext is { } c)
            {
                editorState.OnOpen(projectContext);
            }
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

        public void ChangeState(IEditorState state)
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
