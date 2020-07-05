using Reactive.Bindings;
using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.Common.ViewModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Subjects;
using Reactive.Bindings.Extensions;

namespace Romanesco.ViewModel.States
{
    public class ListViewModel : IStateViewModel
    {
        private readonly ListState state;
        private readonly Subject<Unit> showDetailSubject = new Subject<Unit>();

        public IReadOnlyReactiveProperty<string> Title => state.Title;
        public IReadOnlyReactiveProperty<string> FormattedString => state.FormattedString;
        public IObservable<Unit> ShowDetail => showDetailSubject;
        public ReadOnlyReactiveCollection<IStateViewModel> Elements { get; }
        public Type ElementType => state.ElementType;
        public ReactiveCommand AddCommand { get; } = new ReactiveCommand();
        public ReactiveCommand<int> RemoveCommand { get; } = new ReactiveCommand<int>();
        public ReactiveCommand EditCommand { get; } = new ReactiveCommand();
        public IObservable<Exception> OnError => state.OnError;
        public List<IDisposable> Disposables => state.Disposables;

        public ListViewModel(ListState state, ViewModelInterpretFunc interpreter)
        {
            this.state = state;

            EditCommand.Subscribe(x => showDetailSubject.OnNext(Unit.Default))
	            .AddTo(state.Disposables);

            Elements = state.Elements.ToReadOnlyReactiveCollection(state => interpreter(state));

            state.Disposables.Add(AddCommand);
            state.Disposables.Add(RemoveCommand);
            state.Disposables.Add(EditCommand);
            state.Disposables.Add(Elements);
        }

        public void AddNewElement()
        {
            state.AddNewElement();
        }

        public void RemoveAt(int index)
        {
            state.RemoveAt(index);
        }
    }
}
