using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Reactive.Bindings;
using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.EditorComponents;
using Romanesco.Model.EditorComponents.States;
using Romanesco.Model.Interfaces;

namespace Romanesco.Model.Commands
{
	// ステートが変わった際のイベントのつながり等のギャップを吸収する層
	internal class CommandRouter : ICommandAvailabilityPublisher
	{
		private ReactiveProperty<CommandAvailability> CommandAvailability { get; } = new ReactiveProperty<CommandAvailability>();

		public CommandRouter(IEditorState state)
		{
			CommandAvailability.Value = new CommandAvailability(state);
			CommandAvailability.Value.UpdateCanExecute();

			var canCreateStream = CommandAvailability.SelectMany(x => x.CanCreate);
			CanCreate = new ReactiveProperty<bool>(canCreateStream);
		}

		public void UpdateState(IEditorState state)
		{
			CommandAvailability.Value = new CommandAvailability(state);
			CommandAvailability.Value.UpdateCanExecute();
		}

		public IObservable<IProjectContext> OnCreate => CommandAvailability.Value.OnCreate;

		public IObservable<IProjectContext> OnOpen => CommandAvailability.Value.OnOpen;

		public IObservable<Unit> OnSaveAs => CommandAvailability.Value.OnSaveAs;

		public IReadOnlyReactiveProperty<bool> CanCreate { get; set; }

		public IReadOnlyReactiveProperty<bool> CanOpen => ((ICommandAvailabilityPublisher)CommandAvailability.Value).CanOpen;

		public IReadOnlyReactiveProperty<bool> CanSave => ((ICommandAvailabilityPublisher)CommandAvailability.Value).CanSave;

		public IReadOnlyReactiveProperty<bool> CanSaveAs => ((ICommandAvailabilityPublisher)CommandAvailability.Value).CanSaveAs;

		public IReadOnlyReactiveProperty<bool> CanExport => ((ICommandAvailabilityPublisher)CommandAvailability.Value).CanExport;

		public IReadOnlyReactiveProperty<bool> CanUndo => ((ICommandAvailabilityPublisher)CommandAvailability.Value).CanUndo;

		public IReadOnlyReactiveProperty<bool> CanRedo => ((ICommandAvailabilityPublisher)CommandAvailability.Value).CanRedo;

		public void NotifyEdit() => CommandAvailability.Value.NotifyEdit();

		public Task<IProjectContext?> CreateAsync()
		{
			return ((ICommandInvoker)CommandAvailability.Value).CreateAsync();
		}

		public Task ExportAsync()
		{
			return ((ICommandInvoker)CommandAvailability.Value).ExportAsync();
		}

		public Task<IProjectContext?> OpenAsync()
		{
			return ((ICommandInvoker)CommandAvailability.Value).OpenAsync();
		}

		public void Redo()
		{
			((ICommandInvoker)CommandAvailability.Value).Redo();
		}

		public Task SaveAsAsync()
		{
			return ((ICommandInvoker)CommandAvailability.Value).SaveAsAsync();
		}

		public Task SaveAsync()
		{
			return ((ICommandInvoker)CommandAvailability.Value).SaveAsync();
		}

		public void Undo()
		{
			((ICommandInvoker)CommandAvailability.Value).Undo();
		}
	}
}
