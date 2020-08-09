using System;
using System.Reactive.Linq;
using System.Windows.Input;
using Reactive.Bindings;
using Romanesco.Common.Model.Helpers;

namespace Romanesco.ViewModel.Commands
{
	public abstract class CommandViewModel : ICommand, IDisposable
	{
		private readonly IDisposable subscription;

		protected bool IsCanExecute { get; set; }

		public event EventHandler? CanExecuteChanged;

		public CommandViewModel(IReadOnlyReactiveProperty<bool> canExecuteObservable,
			BooleanUsingScopeSource commandExecution)
		{
			subscription = canExecuteObservable.Concat(commandExecution.IsUsing.Select(x => !x))
				.Subscribe(x =>
				{
					IsCanExecute = x;
					CanExecuteChanged?.Invoke(this, EventArgs.Empty);
				});
			IsCanExecute = canExecuteObservable.Value;
		}

		public virtual bool CanExecute(object? parameter) => IsCanExecute;

		public abstract void Execute(object? parameter);

		public void Dispose()
		{
			subscription.Dispose();
		}
	}
}
