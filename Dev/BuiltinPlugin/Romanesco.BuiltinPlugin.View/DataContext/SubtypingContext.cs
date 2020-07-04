using Reactive.Bindings;
using Romanesco.BuiltinPlugin.ViewModel.States;
using Romanesco.Common.View.Interfaces;
using Romanesco.ViewModel.States;
using System;

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
			vm.CurrentViewModel.Subscribe(x => SetCurrentContext(x, interpreter));
		}

		private void SetCurrentContext(ClassViewModel? vm, ViewInterpretFunc interpreter)
		{
			if (vm is ClassViewModel classVM)
			{
				var stateContext = interpreter(classVM);
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
}
