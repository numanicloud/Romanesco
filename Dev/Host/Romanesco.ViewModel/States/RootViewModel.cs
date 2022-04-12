using Reactive.Bindings;
using Romanesco.Common.ViewModel.Interfaces;

namespace Romanesco.ViewModel.States
{
	internal class RootViewModel
	{
		public ReactiveProperty<IStateViewModel[]> Fields { get; }

		public RootViewModel()
		{
			Fields = new ReactiveProperty<IStateViewModel[]>(new IStateViewModel[0]);
		}
	}
}
