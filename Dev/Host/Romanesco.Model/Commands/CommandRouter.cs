using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Reactive.Bindings;
using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.EditorComponents;
using Romanesco.Model.EditorComponents.States;
using Romanesco.Model.Interfaces;

namespace Romanesco.Model.Commands
{
	internal class CommandRouter : ICommandAvailabilityPublisher
	{
		public CommandAvailability CommandAvailability { get; private set; }

		public CommandRouter(IEditorState state)
		{
			CommandAvailability = new CommandAvailability(state);
			CommandAvailability.UpdateCanExecute();
		}

		public void UpdateState(IEditorState state)
		{
			CommandAvailability = new CommandAvailability(state);
			CommandAvailability.UpdateCanExecute();
		}

		public IObservable<IProjectContext> OnCreate => CommandAvailability.OnCreate;

		public IObservable<IProjectContext> OnOpen => CommandAvailability.OnOpen;

		public IObservable<Unit> OnSaveAs => CommandAvailability.OnSaveAs;

		public IReadOnlyReactiveProperty<bool> CanSave => ((ICommandAvailabilityPublisher)CommandAvailability).CanSave;

		public IReadOnlyReactiveProperty<bool> CanSaveAs => ((ICommandAvailabilityPublisher)CommandAvailability).CanSaveAs;

		public IReadOnlyReactiveProperty<bool> CanExport => ((ICommandAvailabilityPublisher)CommandAvailability).CanExport;

		public IReadOnlyReactiveProperty<bool> CanCreate => ((ICommandAvailabilityPublisher)CommandAvailability).CanCreate;

		public IReadOnlyReactiveProperty<bool> CanOpen => ((ICommandAvailabilityPublisher)CommandAvailability).CanOpen;

		public IReadOnlyReactiveProperty<bool> CanUndo => ((ICommandAvailabilityPublisher)CommandAvailability).CanUndo;

		public IReadOnlyReactiveProperty<bool> CanRedo => ((ICommandAvailabilityPublisher)CommandAvailability).CanRedo;

		public void NotifyEdit() => CommandAvailability.NotifyEdit();

		public Task<IProjectContext?> CreateAsync()
		{
			return ((ICommandInvoker)CommandAvailability).CreateAsync();
		}

		public Task ExportAsync()
		{
			return ((ICommandInvoker)CommandAvailability).ExportAsync();
		}

		public Task<IProjectContext?> OpenAsync()
		{
			return ((ICommandInvoker)CommandAvailability).OpenAsync();
		}

		public void Redo()
		{
			((ICommandInvoker)CommandAvailability).Redo();
		}

		public Task SaveAsAsync()
		{
			return ((ICommandInvoker)CommandAvailability).SaveAsAsync();
		}

		public Task SaveAsync()
		{
			return ((ICommandInvoker)CommandAvailability).SaveAsync();
		}

		public void Undo()
		{
			((ICommandInvoker)CommandAvailability).Undo();
		}
	}
}
