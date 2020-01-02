using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using Reactive.Bindings;

namespace Romanesco.Common.Utility
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
