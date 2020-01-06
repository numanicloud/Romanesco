using Reactive.Bindings;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;
using System;
using System.Reactive;
using System.Reactive.Linq;

namespace Romanesco.Common.ViewModel.Implementations
{
    public class PrimitiveTypeViewModel<T> : IStateViewModel
    {
        public IReadOnlyReactiveProperty<string> Title { get; }

        public IReadOnlyReactiveProperty<string> FormattedString { get; }

        public IFieldState State { get; }

        public ReactiveProperty<T> PrimitiveContent { get; }

        public IObservable<Unit> ShowDetail { get; }

        public IObservable<Exception> OnError => State.OnError;

        public PrimitiveTypeViewModel(IFieldState state, ReactiveProperty<T> primitiveContent)
        {
            State = state;
            PrimitiveContent = primitiveContent;
            Title = state.Title;
            FormattedString = state.FormattedString;
            ShowDetail = Observable.Never<Unit>();
        }
    }
}
