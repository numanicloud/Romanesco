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
	internal sealed class Editor : IEditorFacade, IDisposable
	{
		private IEditorState editorState;
		private readonly CommandAvailability commandAvailability;
		private readonly CommandRouter commandRouter;

		public List<IDisposable> Disposables { get; } = new List<IDisposable>();
		public ReactiveProperty<string> ApplicationTitle { get; } = new ReactiveProperty<string>();
		public ICommandAvailabilityPublisher CommandAvailabilityPublisher => commandRouter;

		public Editor(IEditorStateChanger stateChanger, IEditorState initialState)
		{
			stateChanger.OnChange.Subscribe(ChangeState).AddTo(Disposables);

			// commandAvailability の初期化を保証しなければならないので、ChangeStateをインライン化した
			editorState = initialState;
			UpdateTitle();

			commandRouter = new CommandRouter(initialState);
			SetUpCommand(commandRouter);

			commandAvailability = new CommandAvailability(initialState);
		}

		public void ChangeState(IEditorState state)
		{
			editorState = state;
			UpdateTitle();
			commandRouter.UpdateState(state);
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
			projectContext.ObserveEdit(() => commandRouter.NotifyEdit())
				.AddTo(Disposables);
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
