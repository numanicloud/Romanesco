using Reactive.Bindings;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;
using System;
using System.Reactive;
using System.Reactive.Linq;
using Romanesco.Common.Model.Implementations;

namespace Romanesco.Common.ViewModel.Implementations
{
	public class PrimitiveTypeViewModel<TModel, TInstance> : ProxyViewModelBase<TModel>
		where TModel : PrimitiveTypeState<TInstance>
	{
		public ReactiveProperty<TInstance> PrimitiveContent => State.PrimitiveContent;

        public PrimitiveTypeViewModel(TModel state) : base(state)
		{
		}
	}
}
