using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Romanesco.Common.Model.Helpers;
using Romanesco.Common.ViewModel.Interfaces;
using Romanesco.Model.Interfaces;
using Romanesco.ViewModel.States;

namespace Romanesco.ViewModel.Commands
{
	internal class CommandManagerViewModel : IDisposable
	{
		private readonly List<IDisposable> disposables = new List<IDisposable>();

		public ReactiveProperty<IStateViewModel[]> Roots { get; }
		public BooleanUsingScopeSource CommandExecution { get; } = new BooleanUsingScopeSource();
		public ICommand Create { get; }
		public ICommand Open { get; }
		public ICommand Save { get; }
		public ICommand SaveAs { get; }
		public ICommand Export { get; }
		public ICommand Undo { get; }
		public ReactiveCommand Redo { get; }

		public CommandManagerViewModel(ICommandAvailabilityPublisher model,
			ReactiveProperty<IStateViewModel[]> roots, IViewModelInterpreter interpreter)
		{
			Create = new CreateCommandViewModel(model, roots, interpreter, CommandExecution);
			Open = new OpenCommandViewModel(model, CommandExecution, roots, interpreter);
			Save = new SaveCommandViewModel(model, CommandExecution);
			SaveAs = new SaveAsCommandViewModel(model, CommandExecution);
			Export = new ExportCommandViewModel(model, CommandExecution);
			Undo = new UndoCommandViewModel(model, CommandExecution);

			Redo = ToEditorCommand(model.CanRedo);
			
			Redo.SubscribeSafe(x => model.Redo())
				.AddTo(disposables);

			Roots = roots;
		}
		
		private ReactiveCommand ToEditorCommand(IObservable<bool> stream)
		{
			var canExecute = stream
				.Concat(CommandExecution.IsUsing.Select(x => !x));
			var scheduler = new SynchronizationContextScheduler(SynchronizationContext.Current);
			return new ReactiveCommand(canExecute, scheduler);
		}

		public void Dispose()
		{
			disposables.ForEach(x => x.Dispose());
		}
	}
}
