using System;
using System.Collections.ObjectModel;
using NacHelpers.Extensions;
using Reactive.Bindings;
using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.ViewModel.Implementations;

namespace Romanesco.BuiltinPlugin.ViewModel.States;

public class IntIdChoiceViewModel : ProxyViewModelBase<IntIdChoiceState>
{
	public ObservableCollection<IFieldState>? Choices { get; private set; }
	public ReactiveProperty<IFieldState?> SelectedItem { get; }

	public IntIdChoiceViewModel(IntIdChoiceState state) : base(state)
	{
		SelectedItem = state.SelectedItem;

		state.Master.FilterNullRef()
			.Subscribe(list => Choices = list.State.Elements)
			.AddTo(state.Disposables);
	}
}