using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using NacHelpers.Extensions;
using Reactive.Bindings;
using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.ViewModel.Implementations;

namespace Romanesco.BuiltinPlugin.ViewModel.States
{
	public class IntIdChoiceListViewModel : ProxyViewModelBase<IntIdChoiceListState>
	{
		public ObservableCollection<IFieldState>? Choices { get; private set; }
		public ReadOnlyObservableCollection<IntIdChoiceState> Elements => State.ChoiceStates;
		public ReactiveCommand AddCommand { get; } = new();
		public ReactiveCommand<IntIdChoiceState> RemoveCommand { get; } = new ();
		public ReactiveCommand EditCommand { get; } = new();
		public List<IDisposable> Disposables => State.Disposables;

		public IntIdChoiceListViewModel(IntIdChoiceListState state)
			: base(state)
		{
			EditCommand.Subscribe(_ => ShowDetailSubject.OnNext(Unit.Default))
				.AddTo(Disposables);

			state.Master.FilterNullRef()
				.Subscribe(list => Choices = list.State.Elements)
				.AddTo(state.Disposables);
		}

		public void AddNewElement()
		{
			State.AddNewElement();
		}

		public void RemoveAt(int index)
		{
			State.RemoveAt(index);
		}
	}
}
