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

		public async Task<IProjectContext?> CreateAsync()
		{
			if (!(await editorState.GetLoadService().CreateAsync() is { } projectContext))
			{
				return null;
			}

            UpdateTitle();
			ObserveEdit(projectContext);
			editorState.OnCreate(projectContext);

			return projectContext;
		}

		public async Task<IProjectContext?> OpenAsync()
        {
	        if (!(await editorState.GetLoadService().OpenAsync() is {} projectContext))
	        {
		        return null;
	        }

	        UpdateTitle();
            ObserveEdit(projectContext);
            editorState.OnOpen(projectContext);

            return projectContext;
        }

		private void ObserveEdit(IProjectContext projectContext)
		{
			projectContext.ObserveEdit(OnEdit).AddTo(Disposables);
		}

		private void OnEdit()
		{
			editorState.OnEdit();
            editorState.UpdateHistoryAvailability(canExecuteSubject);
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
            editorState.UpdateCanExecute(canExecuteSubject);
        }

        private void UpdateTitle() => ApplicationTitle.Value = editorState.Title;
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
