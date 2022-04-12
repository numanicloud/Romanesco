using Reactive.Bindings;
using Romanesco.BuiltinPlugin.ViewModel.States;
using Romanesco.Common.View.Interfaces;
using System;
using Reactive.Bindings.Extensions;

namespace Romanesco.BuiltinPlugin.View.DataContext
{
	public class SubtypingContext
	{
		public ReactiveProperty<ClassContext?> CurrentContext { get; } = new ReactiveProperty<ClassContext?>();
		public SubtypingClassViewModel ViewModel { get; set; }
		public IReadOnlyReactiveProperty<string> Title => ViewModel.Title;

		public SubtypingContext(SubtypingClassViewModel vm, ViewInterpretFunc interpreter)
		{
			ViewModel = vm;

			SetCurrentContext(vm.CurrentViewModel.Value, interpreter);
			vm.CurrentViewModel.Subscribe(x => SetCurrentContext(x, interpreter))
				.AddTo(vm.Disposables);

			vm.Disposables.Add(CurrentContext);
		}

		private void SetCurrentContext(ClassViewModel? vm, ViewInterpretFunc interpreter)
		{
			if (vm is null) return;

			var stateContext = interpreter(vm);
			if (stateContext.BlockControl.DataContext is ClassContext context)
			{
				CurrentContext.Value = context;
			}
			else
			{
				throw new Exception("予期せぬエラーが発生しました。 ClassViewModel から ClassContext が生成されませんでした。");
			}
		}
	}
}
