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

		protected CommandViewModel(IReadOnlyReactiveProperty<bool> canExecuteObservable,
			BooleanUsingScopeSource commandExecution)
		{
			var lastCanExecuteState = false;
			var capture = this;

			// IsUsingからfalseが流れてきたらfalseにしたい。
			// コマンド実行中は他のコマンドを使えないようにするコードだが、これはModel側かも
			subscription = canExecuteObservable.CombineLatest(commandExecution.IsUsing.Select(x => !x), (b, b1) =>
				{
					Console.WriteLine(capture);
					return b && b1;
				})
				.Subscribe(x =>
				{
					IsCanExecute = x;
					lastCanExecuteState = IsCanExecute;
					CanExecuteChanged?.Invoke(this, EventArgs.Empty);
				});

			IsCanExecute = canExecuteObservable.Value;
			lastCanExecuteState = IsCanExecute;
		}

		public virtual bool CanExecute(object? parameter) => IsCanExecute;

		public abstract void Execute(object? parameter);

		public void Dispose()
		{
			subscription.Dispose();
		}
	}
}
