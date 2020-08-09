using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Reactive.Bindings;
using Romanesco.Common.Model.Helpers;
using Romanesco.Common.ViewModel.Interfaces;
using Romanesco.Model.Interfaces;
using Romanesco.ViewModel.States;

namespace Romanesco.ViewModel.Commands
{
	internal class OpenCommandViewModel : ICommand, IDisposable
	{
		public event EventHandler? CanExecuteChanged;

		private readonly IDisposable subscription;
		private readonly ICommandAvailabilityPublisher model;
		private readonly BooleanUsingScopeSource commandExecution;
		private readonly ReactiveProperty<IStateViewModel[]> roots;
		private readonly IViewModelInterpreter interpreter;
		private bool isCanExecute;

		public OpenCommandViewModel(ICommandAvailabilityPublisher model, BooleanUsingScopeSource commandExecution,
			ReactiveProperty<IStateViewModel[]> roots, IViewModelInterpreter interpreter)
		{
			subscription = model.CanOpen
				.Concat(commandExecution.IsUsing.Select(x => !x))
				.Subscribe(x =>
				{
					isCanExecute = true;
					CanExecuteChanged?.Invoke(this, EventArgs.Empty);
				});
			isCanExecute = model.CanOpen.Value;
			this.model = model;
			this.commandExecution = commandExecution;
			this.roots = roots;
			this.interpreter = interpreter;
		}

		public bool CanExecute(object? parameter) => isCanExecute;

		public void Execute(object? parameter) => OpenAsync().Forget();
		
		private async Task OpenAsync()
		{
			using (commandExecution.Create())
			{
				var projectContext = await model.OpenAsync();
				if (projectContext == null)
				{
					return;
				}

				roots.Value = projectContext.Project.Root.States
					.Select(s => interpreter.InterpretAsViewModel(s))
					.ToArray();
			}
		}

		public void Dispose()
		{
			subscription.Dispose();
		}
	}
}
