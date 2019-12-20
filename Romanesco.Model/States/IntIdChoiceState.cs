using System.Reactive;
using System;
using Romanesco.Common.Utility;
using Reactive.Bindings;

namespace Romanesco.Model.States
{
    public class IntIdChoiceState : Romanesco.Common.IFieldState
    {
        public ReactiveProperty<string> Title { get; }
        public ReactiveProperty<object> Content { get; }
        public ReactiveProperty<string> FormattedString { get; }
        public Type Type { get; }
        public ValueSettability Settability { get; }
        public IObservable<Exception> OnError { get; }
    }
}