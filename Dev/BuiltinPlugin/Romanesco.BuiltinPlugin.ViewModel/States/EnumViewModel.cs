using System;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.Common.ViewModel.Implementations;

namespace Romanesco.BuiltinPlugin.ViewModel.States;

public class EnumViewModel : ProxyViewModelBase<EnumState>
{
	public ReactiveProperty<object> Content => State.Content;
	public object[] Choices => State.Choices;

	public object Selected { get; set; }

	public EnumViewModel(EnumState state) : base(state)
	{
		Selected = state.Choices[0];
		Content.Subscribe(x => Selected = x)
			.AddTo(state.Disposables);
	}
}