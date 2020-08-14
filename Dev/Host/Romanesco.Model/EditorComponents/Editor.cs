using Reactive.Bindings;
using Romanesco.Model.EditorComponents.States;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Reactive.Bindings.Extensions;
using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.Commands;
using Romanesco.Model.Interfaces;

namespace Romanesco.Model.EditorComponents
{
	internal sealed class Editor : IEditorFacade, IDisposable, IEditorStateRepository
	{
		private readonly ReactiveProperty<IEditorState> _editorState = new ReactiveProperty<IEditorState>();
		private readonly CommandAvailability _commandAvailability;
		private readonly CommandRouter _commandRouter;

		public List<IDisposable> Disposables { get; } = new List<IDisposable>();
		public ReactiveProperty<string> ApplicationTitle { get; } = new ReactiveProperty<string>();
		public ICommandAvailabilityPublisher CommandAvailabilityPublisher => _commandRouter;
		public IReadOnlyReactiveProperty<IEditorState> EditorState => _editorState;

		public Editor(IEditorStateChanger stateChanger, IEditorState initialState)
		{
			stateChanger.OnChange.Subscribe(ChangeState).AddTo(Disposables);

			// commandAvailability の初期化を保証しなければならないので、ChangeStateをインライン化した
			_editorState.Value = initialState;
			UpdateTitle();

			_commandRouter = new CommandRouter(initialState);
			SetUpCommand(_commandRouter);

			_commandAvailability = new CommandAvailability(initialState);
		}

		public void ChangeState(IEditorState state)
		{
			_editorState.Value = state;
			UpdateTitle();
			_commandRouter.UpdateState(state);
		}

		private void SetUpCommand(CommandRouter target)
		{
			target.OnCreate.Subscribe(SetProject);
			target.OnOpen.Subscribe(SetProject);
			target.OnSaveAs.Subscribe(x => UpdateTitle());
		}

		private void SetProject(IProjectContext projectContext)
		{
			UpdateTitle();
			ObserveEdit(projectContext);
		}

		private void ObserveEdit(IProjectContext projectContext)
		{
			projectContext.ObserveEdit(() => _commandRouter.NotifyEdit())
				.AddTo(Disposables);
		}

		private void UpdateTitle() => ApplicationTitle.Value = _editorState.Value.Title;

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
