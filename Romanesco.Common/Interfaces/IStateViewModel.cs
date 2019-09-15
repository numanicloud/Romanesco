using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;

namespace Romanesco.Common {
    public interface IStateViewModel {
        ReactiveProperty<string> Title { get; }
        ReactiveProperty<object> Content { get; }
        ReactiveProperty<string> FormattedString { get; }
        IObservable<Unit> ShowDetail { get; }
    }
}
