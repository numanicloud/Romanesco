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
	public class ListViewModel : ProxyViewModelBase<ListState>
	{
		public ReadOnlyReactiveCollection<IStateViewModel> Elements { get; }
		public Type ElementType => State.ElementType;
		public ReactiveCommand AddCommand { get; } = new ReactiveCommand();
		public ReactiveCommand<int> RemoveCommand { get; } = new ReactiveCommand<int>();
		public ReactiveCommand EditCommand { get; } = new ReactiveCommand();
		public List<IDisposable> Disposables => State.Disposables;

        public ListViewModel(ListState state, ViewModelInterpretFunc interpreter)
	        : base(state)
        {
	        EditCommand.Subscribe(x => ShowDetailSubject.OnNext(Unit.Default))
		        .AddTo(state.Disposables);

	        Elements = state.Elements
		        .ToReadOnlyReactiveCollection(state => interpreter(state));

            state.Disposables.AddRange(new IDisposable[]
            {
                AddCommand, RemoveCommand, EditCommand, Elements
            });
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
