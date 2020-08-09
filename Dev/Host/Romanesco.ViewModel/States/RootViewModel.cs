using Reactive.Bindings;
using Romanesco.Common.ViewModel.Interfaces;

namespace Romanesco.ViewModel.States
{
	internal class RootViewModel
	{
		private readonly ReactiveProperty<IStateViewModel[]> mutableRoots;

		public IReadOnlyReactiveProperty<IStateViewModel[]> Roots => mutableRoots;

		public RootViewModel()
		{
			mutableRoots = new ReactiveProperty<IStateViewModel[]>(new IStateViewModel[0]);
		}
	}
}
