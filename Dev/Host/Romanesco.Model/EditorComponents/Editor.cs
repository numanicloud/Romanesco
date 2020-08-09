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
			SetUpCommand(commandAvailability);
		}

		public void ChangeState(IEditorState state)
		{
			editorState = state;
			UpdateTitle();

			commandAvailability = new CommandAvailability(state);
			commandAvailability.UpdateCanExecute();
			SetUpCommand(commandAvailability);
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

		private void ObserveEdit(IProjectContext projectContext)
		{
			projectContext.ObserveEdit(() => commandAvailability.NotifyEdit())
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
