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
		private readonly CommandAvailability commandAvailability_xxx;

		public List<IDisposable> Disposables { get; } = new List<IDisposable>();
		public ReactiveProperty<string> ApplicationTitle { get; } = new ReactiveProperty<string>();
		public IObservable<(EditorCommandType command, bool canExecute)> CanExecuteObservable { get; }
		public ICommandAvailabilityPublisher CommandAvailability { get; }

		public Editor(IEditorStateChanger stateChanger, IEditorState initialState, CommandAvailability commandAvailability)
		{
			// TODO: コンストラクタパラメータを EditorSession にする

			CanExecuteObservable = commandAvailability.Observable;
			CommandAvailability = commandAvailability;
			commandAvailability_xxx = commandAvailability;

			stateChanger.OnChange.Subscribe(ChangeState).AddTo(Disposables);
			editorState = initialState;
			ChangeState(editorState);
		}

		public async Task<IProjectContext?> CreateAsync()
		{
			if (!(await commandAvailability_xxx.CreateAsync(editorState) is { } projectContext))
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
			if (!(await commandAvailability_xxx.OpenAsync(editorState) is {} projectContext))
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
			editorState.NotifyEdit();
		}

		public async Task SaveAsync()
		{
			await commandAvailability_xxx.SaveAsync(editorState);
		}

		public async Task SaveAsAsync()
		{
			await commandAvailability_xxx.SaveAsAsync(editorState);
			UpdateTitle();
		}

		public async Task ExportAsync()
		{
			await commandAvailability_xxx.ExportAsync(editorState);
		}

		public void Undo()
		{
			commandAvailability_xxx.Undo(editorState);
		}

		public void Redo()
		{
			commandAvailability_xxx.Redo(editorState);
		}

		public void ChangeState(IEditorState state)
		{
			editorState = state;
			UpdateTitle();
			commandAvailability_xxx.UpdateCanExecute(state);
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
