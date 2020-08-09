using Reactive.Bindings;
using Romanesco.Model.EditorComponents.States;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Security.Cryptography.X509Certificates;
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
		private CommandAvailability commandAvailability;

		public List<IDisposable> Disposables { get; } = new List<IDisposable>();
		public ReactiveProperty<string> ApplicationTitle { get; } = new ReactiveProperty<string>();
		public ICommandAvailabilityPublisher CommandAvailabilityPublisher => commandAvailability;

		public Editor(IEditorStateChanger stateChanger, IEditorState initialState)
		{
			stateChanger.OnChange.Subscribe(ChangeState).AddTo(Disposables);

			// commandAvailability の初期化を保証しなければならないので、ChangeStateをインライン化した
			editorState = initialState;
			UpdateTitle();
			commandAvailability = new CommandAvailability(initialState);
			commandAvailability.UpdateCanExecute();
		}

		private void SetUpCommand(CommandAvailability target)
		{
			target.OnCreate.Subscribe(SetProject);
			target.OnOpen.Subscribe(SetProject);
			target.OnSaveAs.Subscribe(x => UpdateTitle());
		}

		private void SetProject(IProjectContext projectContext)
		{
			UpdateTitle();
			ObserveEdit(projectContext);
			editorState.OnCreate(projectContext);
		}

		public async Task<IProjectContext?> CreateAsync()
		{
			if (!(await commandAvailability.CreateAsync() is { } projectContext))
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
			if (!(await commandAvailability.OpenAsync() is {} projectContext))
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
			commandAvailability.NotifyEdit();
		}

		public async Task SaveAsAsync()
		{
			await commandAvailability.SaveAsAsync();
			UpdateTitle();
		}

		public void ChangeState(IEditorState state)
		{
			editorState = state;
			UpdateTitle();

			commandAvailability = new CommandAvailability(state);
			commandAvailability.UpdateCanExecute();
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
