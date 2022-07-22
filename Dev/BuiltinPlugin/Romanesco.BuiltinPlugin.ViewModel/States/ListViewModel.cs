using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.Common.Model.Exceptions;
using Romanesco.Common.ViewModel.Implementations;
using Romanesco.Common.ViewModel.Interfaces;

namespace Romanesco.BuiltinPlugin.ViewModel.States;

public class ListViewModel : ProxyViewModelBase<ListState>
{
	public ReadOnlyReactiveCollection<IStateViewModel> Elements { get; }
	public Type ElementType => State.ElementType;
	public ReactiveCommand AddCommand { get; } = new();
	public ReactiveCommand<int> RemoveCommand { get; } = new();
	public ReactiveCommand EditCommand { get; } = new();
	public ReactiveCommand<int> MoveUpCommand { get; } = new();
	public ReactiveCommand<int> MoveDownCommand { get; } = new();
	public List<IDisposable> Disposables => State.Disposables;
	public override IObservable<Exception> OnError { get; }
	public IReactiveProperty<int> SelectedIndex { get; } = new ReactiveProperty<int>(-1);

	public ListViewModel(ListState state, ViewModelInterpretFunc interpreter)
		: base(state)
	{
		EditCommand.Subscribe(x => ShowDetailSubject.OnNext(Unit.Default))
			.AddTo(state.Disposables);

		Elements = state.Elements
			.ToReadOnlyReactiveCollection(state => interpreter(state), Scheduler.CurrentThread)
			.AddTo(Disposables);

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

		RemoveCommand.Where(i => i < Elements.Count)
			.Subscribe(index =>
			{
				try
				{
					state.RemoveAt(index);
				}
				catch (Exception ex)
				{
					onError.OnNext(ContentAccessException.GetListError(ex));
				}
			}).AddTo(Disposables);

		MoveUpCommand.Subscribe(index =>
		{
			try
			{
				var newIndex = state.MoveUp(index);
			}
			catch (Exception e)
			{
				onError.OnNext(ContentAccessException.GetListError(e));
			}
		}).AddTo(Disposables);

		MoveDownCommand.Subscribe(index =>
		{
			try
			{
				var newIndex = state.MoveDown(index);
			}
			catch (Exception ex)
			{
				onError.OnNext(ContentAccessException.GetListError(ex));
			}
		}).AddTo(Disposables);

		OnError = state.OnError.Merge(onError);

		state.Disposables.AddRange(new IDisposable[]
		{
			AddCommand, RemoveCommand, EditCommand,
			Elements, MoveUpCommand, MoveDownCommand
		});
	}
}