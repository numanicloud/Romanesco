using Reactive.Bindings;
using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.Common.ViewModel.Implementations;

namespace Romanesco.BuiltinPlugin.ViewModel.States
{
    public class EnumViewModel : ProxyViewModelBase<EnumState>
    {
	    public ReactiveProperty<object> Content => State.Content;
	    public object[] Choices => State.Choices;

	    public EnumViewModel(EnumState state) : base(state)
	    {
	    }
    }
}
