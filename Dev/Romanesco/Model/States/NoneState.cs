using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using Reactive.Bindings;
using Romanesco.Common.Utility;

namespace Romanesco.Model.States
{
    public class NoneState : Common.IFieldState
    {
        public ReactiveProperty<string> Title { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<object> Content { get; } = new ReactiveProperty<object>();
        public IReadOnlyReactiveProperty<string> FormattedString { get; } = new ReactiveProperty<string>();
        public Type Type => typeof(object);
        public ValueSettability Settability { get; } = null;
        public IObservable<Exception> OnError => Observable.Never<Exception>();
        public IObservable<Unit> OnEdited => Observable.Never<Unit>();
    }
}
