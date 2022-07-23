using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.Common.ViewModel.Implementations;
using Romanesco.Common.ViewModel.Interfaces;

namespace Romanesco.BuiltinPlugin.ViewModel.States;

public class ClassViewModel : ProxyViewModelBase<ClassState>, IOpenCommandConsumer
{
	public record Property(string Name, IStateViewModel ViewModel);

	public IStateViewModel[] Children { get; }
	public Property[] Properties { get; }
	public ReactiveCommand EditCommand { get; } = new();
	public ReactiveCommand OnOpenCommand { get; } = new();
	public List<IDisposable> Disposables => State.Disposables;
	public ReactiveProperty<IStateViewModel?> CloseUpViewModel { get; } = new();

	public ClassViewModel(ClassState state, Property[] properties)
		: base(state)
	{
		Children = properties.Select(x => x.ViewModel).ToArray();
		Properties = properties;

		EditCommand.Subscribe(_ => ShowDetailSubject.OnNext(Unit.Default))
			.AddTo(Disposables);

		EditCommand.AddTo(Disposables);
		OnOpenCommand.AddTo(Disposables);
	}
}