using System;
using System.Collections.Generic;
using System.Reactive;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.Common.ViewModel.Implementations;
using Romanesco.Common.ViewModel.Interfaces;

namespace Romanesco.BuiltinPlugin.ViewModel.States;

public class ClassViewModel : ProxyViewModelBase<ClassState>, IOpenCommandConsumer
{
	public IStateViewModel[] Fields { get; }
	public ReactiveCommand EditCommand { get; } = new();
	public ReactiveCommand OnOpenCommand { get; } = new();
	public List<IDisposable> Disposables => State.Disposables;
	public ReactiveProperty<IStateViewModel?> CloseUpViewModel { get; } = new();

	public ClassViewModel(ClassState state, IStateViewModel[] fields)
		: base(state)
	{
		Fields = fields;
		EditCommand.Subscribe(_ => ShowDetailSubject.OnNext(Unit.Default))
			.AddTo(Disposables);

		EditCommand.AddTo(Disposables);
		OnOpenCommand.AddTo(Disposables);
	}
}

public class ClassInlineViewModel : ClassViewModel
{
	public ClassInlineViewModel(ClassState state, IStateViewModel[] fields)
		: base(state, fields)
	{
	}
}

public class ClassBlockViewModel : ClassViewModel
{
	public ReactiveProperty<IStateViewModel?> CloseUpViewModel { get; } = new();

	public ClassBlockViewModel(ClassState state, IStateViewModel[] fields)
		: base(state, fields)
	{
	}
}