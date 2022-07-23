using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.Common.Model.Exceptions;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.ViewModel.Implementations;
using Romanesco.BuiltinPlugin.Model;

namespace Romanesco.BuiltinPlugin.ViewModel.States;

public class IntIdChoiceListViewModel : ProxyViewModelBase<IntIdChoiceListState>
{
	public ObservableCollection<IFieldState>? Choices { get; private set; }
	public ReadOnlyObservableCollection<IntIdChoiceState> Elements => State.Elements;
	public ReactiveCommand AddCommand { get; } = new();
	public ReactiveCommand<IntIdChoiceState> RemoveCommand { get; } = new ();
	public ReactiveCommand EditCommand { get; } = new();
	public List<IDisposable> Disposables => State.Disposables;

	public IntIdChoiceListViewModel(IntIdChoiceListState state)
		: base(state)
	{
		var onError = new Subject<Exception>();

		AddCommand.Subscribe(_ =>
		{
			try
			{
				state.AddNewElement();
			}
			catch (Exception ex)
			{
				onError.OnNext(ContentAccessException.GetListError(ex));
			}
		}).AddTo(Disposables);

		RemoveCommand.Subscribe(i =>
		{
			try
			{
				var index = Elements.IndexOf(i);
				if (index != -1)
				{
					state.RemoveAt(index);
				}
			}
			catch (Exception ex)
			{
				onError.OnNext(ContentAccessException.GetListError(ex));
			}
		}).AddTo(Disposables);

		OnError = OnError.Merge(onError);

		EditCommand.Subscribe(_ => ShowDetailSubject.OnNext(Unit.Default))
			.AddTo(Disposables);

		// TODO: Masterの中身が0個の場合に更新してくれない
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