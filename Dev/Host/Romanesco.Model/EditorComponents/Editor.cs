using Reactive.Bindings;
using Romanesco.Model.EditorComponents.States;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Reactive.Bindings.Extensions;
using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.Commands;

namespace Romanesco.Model.EditorComponents
{
	internal sealed class Editor : IEditorFacade, IDisposable
	{
		private readonly CommandAvailability commandAvailability;
		private IEditorState editorState;
		public List<IDisposable> Disposables { get; } = new List<IDisposable>();

		public ReactiveProperty<string> ApplicationTitle { get; } = new ReactiveProperty<string>();
		public IObservable<(EditorCommandType command, bool canExecute)> CanExecuteObservable
			=> commandAvailability.Observable;

		public Editor(IEditorStateChanger stateChanger, IEditorState initialState, CommandAvailability commandAvailability)
		{
			stateChanger.OnChange.Subscribe(ChangeState).AddTo(Disposables);

			editorState = initialState;
			ChangeState(editorState);

			this.commandAvailability = commandAvailability;
		}

		public async Task<IProjectContext?> CreateAsync()
		{
			if (!(await editorState.CreateAsync() is { } projectContext))
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
			if (!(await editorState.OpenAsync() is {} projectContext))
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
			editorState.NotifyEdit(commandAvailability);
		}

		public async Task SaveAsync()
		{
			await editorState.SaveAsync();
		}

		public async Task SaveAsAsync()
		{
			await editorState.SaveAsAsync();
			UpdateTitle();
		}

		public async Task ExportAsync()
		{
			await editorState.ExportAsync();
		}

		public void Undo()
		{
			editorState.Undo(commandAvailability);
		}

		public void Redo()
		{
			editorState.Redo(commandAvailability);
		}

		public void ChangeState(IEditorState state)
		{
			editorState = state;
			UpdateTitle();
			editorState.UpdateCanExecute(commandAvailability);
		}

		private void UpdateTitle() => ApplicationTitle.Value = editorState.Title;
		public void Dispose()
		{
			ApplicationTitle.Dispose();
			foreach (var disposable in Disposables)
			{
				disposable.Dispose();
			}
		}
	}
}
