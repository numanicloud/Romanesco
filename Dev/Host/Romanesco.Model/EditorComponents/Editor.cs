using Reactive.Bindings;
using Romanesco.Model.EditorComponents.States;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Reactive.Bindings.Extensions;
using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.Commands;
using Romanesco.Model.Interfaces;

namespace Romanesco.Model.EditorComponents
{
	internal sealed class Editor : IEditorFacade, IDisposable
	{
		private IEditorState editorState;
		private readonly CommandAvailability commandAvailability;

		public List<IDisposable> Disposables { get; } = new List<IDisposable>();
		public ReactiveProperty<string> ApplicationTitle { get; } = new ReactiveProperty<string>();
		public IObservable<(EditorCommandType command, bool canExecute)> CanExecuteObservable { get; }
		public ICommandAvailabilityPublisher CommandAvailabilityPublisher { get; }

		public Editor(IEditorStateChanger stateChanger, IEditorState initialState)
		{
			// TODO: コンストラクタパラメータを EditorSession にする

			commandAvailability = new CommandAvailability(initialState);
			CanExecuteObservable = commandAvailability.Observable;
			CommandAvailabilityPublisher = commandAvailability;

			stateChanger.OnChange.Subscribe(ChangeState).AddTo(Disposables);
			editorState = initialState;
			ChangeState(editorState);
		}

		public async Task<IProjectContext?> CreateAsync()
		{
			if (!(await commandAvailability.CreateAsync(editorState) is { } projectContext))
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
			if (!(await commandAvailability.OpenAsync(editorState) is {} projectContext))
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

		/* 各コマンドの実行リクエストを受け付ける */
		private void OnEdit()
		{
			commandAvailability.NotifyEdit(editorState);
		}

		public async Task SaveAsync()
		{
			await commandAvailability.SaveAsync(editorState);
		}

		public async Task SaveAsAsync()
		{
			await commandAvailability.SaveAsAsync(editorState);
			UpdateTitle();
		}

		public async Task ExportAsync()
		{
			await commandAvailability.ExportAsync(editorState);
		}

		public void Undo()
		{
			commandAvailability.Undo(editorState);
		}

		public void Redo()
		{
			commandAvailability.Redo(editorState);
		}

		public void ChangeState(IEditorState state)
		{
			editorState = state;
			UpdateTitle();
			commandAvailability.UpdateCanExecute(state);
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
