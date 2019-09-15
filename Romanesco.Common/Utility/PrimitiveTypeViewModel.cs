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
        public ReactiveProperty<string> Title { get; }

        public ReactiveProperty<object> Content { get; }

        public ReactiveProperty<string> FormattedString { get; }

        public IFieldState State { get; }

        public ReactiveProperty<T> PrimitiveContent { get; }

        public IObservable<Unit> ShowDetail { get; }

        public PrimitiveTypeViewModel(IFieldState state, ReactiveProperty<T> primitiveContent)
        {
            State = state;
            PrimitiveContent = primitiveContent;
            Title = state.Title;
            Content = state.Content;
            FormattedString = state.FormattedString;
            ShowDetail = Observable.Never<Unit>();
        }
    }
}
