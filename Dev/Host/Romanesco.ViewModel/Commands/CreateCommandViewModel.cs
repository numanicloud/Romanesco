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
	class CreateCommandViewModel : ICommand, IDisposable
	{
		private readonly ICommandAvailabilityPublisher model;
		private readonly IViewModelInterpreter interpreter;
		private readonly ReactiveProperty<IStateViewModel[]> roots;
		private readonly BooleanUsingScopeSource commandExecution;
		private readonly IDisposable subscription;

		private bool isCanExecute;

		public CreateCommandViewModel(ICommandAvailabilityPublisher model,
			ReactiveProperty<IStateViewModel[]> roots,
			IViewModelInterpreter interpreter,
			BooleanUsingScopeSource commandExecution)
		{
			this.model = model;
			this.interpreter = interpreter;
			this.commandExecution = commandExecution;
			this.roots = roots;

			subscription = model.CanCreate.Concat(commandExecution.IsUsing.Select(x => !x))
				.Subscribe(x =>
			{
				isCanExecute = x;
				CanExecuteChanged?.Invoke(this, EventArgs.Empty);
			});
			isCanExecute = model.CanCreate.Value;
		}

		public bool CanExecute(object? parameter) => isCanExecute;

		public void Execute(object? parameter) => CreateAsync().Forget();

		private async Task CreateAsync()
		{
			// 実行をロックする機能は IProjectLoadService 側に入れるべきかも
			using (commandExecution.Create())
			{
				var projectContext = await model.CreateAsync();
				if (projectContext != null)
				{
					roots.Value = projectContext.Project.Root.States
						.Select(s => interpreter.InterpretAsViewModel(s))
						.ToArray();
				}
			}
		}

		public void Dispose()
		{
			subscription.Dispose();
		}

		public event EventHandler? CanExecuteChanged;
	}
}
