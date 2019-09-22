using Reactive.Bindings;
using Romanesco.Common.Utility;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;

namespace Romanesco.Common {
    public interface IFieldState {
        ReactiveProperty<string> Title { get; }
        ReactiveProperty<object> Content { get; }
        ReactiveProperty<string> FormattedString { get; }
        Type Type { get; }
        ValueSettability Settability { get; }
        IObservable<Exception> OnError { get; }
    }
}
