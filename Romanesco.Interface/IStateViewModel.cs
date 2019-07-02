using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Interface {
    public interface IStateViewModel {
        ReactiveProperty<string> Title { get; }
        ReactiveProperty<object> Content { get; }
        ReactiveProperty<string> FormattedString { get; }
    }
}
