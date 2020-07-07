using System;
using System.Collections.Generic;
using System.Reactive;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.Common.ViewModel.Implementations;
using Romanesco.Common.ViewModel.Interfaces;

namespace Romanesco.BuiltinPlugin.ViewModel.States
{
	public class ClassViewModel : ProxyViewModelBase<ClassState>
	{
		public IStateViewModel[] Fields { get; }
		public ReactiveCommand EditCommand { get; }
		public List<IDisposable> Disposables => State.Disposables;

        public ClassViewModel(ClassState state, IStateViewModel[] fields)
			: base(state)
		{
			Fields = fields;
			EditCommand = new ReactiveCommand();
			EditCommand.Subscribe(_ => ShowDetailSubject.OnNext(Unit.Default))
				.AddTo(Disposables);
			EditCommand.AddTo(Disposables);
		}
	}
}
