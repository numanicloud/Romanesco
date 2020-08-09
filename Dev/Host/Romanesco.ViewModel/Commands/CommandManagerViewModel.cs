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
		private readonly ICommandAvailabilityPublisher model;
		private readonly List<IDisposable> disposables = new List<IDisposable>();

		public ReactiveProperty<IStateViewModel[]> Roots { get; }
		public BooleanUsingScopeSource CommandExecution { get; } = new BooleanUsingScopeSource();
		public ICommand Create { get; }
		public ICommand Open { get; }
		public ICommand Save { get; }
		public ReactiveCommand SaveAs { get; }
		public ReactiveCommand Export { get; }
		public ReactiveCommand Undo { get; }
		public ReactiveCommand Redo { get; }

		public CommandManagerViewModel(ICommandAvailabilityPublisher model,
			ReactiveProperty<IStateViewModel[]> roots, IViewModelInterpreter interpreter)
		{
			Create = new CreateCommandViewModel(model, roots, interpreter, CommandExecution);
			Open = new OpenCommandViewModel(model, CommandExecution, roots, interpreter);
			Save = new SaveCommandViewModel(model, CommandExecution);

			SaveAs = ToEditorCommand(model.CanSaveAs);
			Export = ToEditorCommand(model.CanExport);
			Undo = ToEditorCommand(model.CanUndo);
			Redo = ToEditorCommand(model.CanRedo);
			
			SaveAs.SubscribeSafe(x => SaveAsAsync().Wait())
				.AddTo(disposables);
			
			Export.SubscribeSafe(x => ExportAsync().Forget())
				.AddTo(disposables);
			
			Undo.SubscribeSafe(x => model.Undo())
				.AddTo(disposables);
			
			Redo.SubscribeSafe(x => model.Redo())
				.AddTo(disposables);

			this.model = model;
			Roots = roots;
		}
		
		private ReactiveCommand ToEditorCommand(IObservable<bool> stream)
		{
			var canExecute = stream
				.Concat(CommandExecution.IsUsing.Select(x => !x));
			var scheduler = new SynchronizationContextScheduler(SynchronizationContext.Current);
			return new ReactiveCommand(canExecute, scheduler);
		}

		private async Task SaveAsAsync()
		{
			using (CommandExecution.Create())
			{
				await model.SaveAsAsync();
			}
		}

		private async Task ExportAsync()
		{
			using (CommandExecution.Create())
			{
				await model.ExportAsync();
			}
		}

		public void Dispose()
		{
			disposables.ForEach(x => x.Dispose());
		}
	}
}
