using System;
using System.Collections.Generic;
using System.Text;
using Reactive.Bindings;

namespace Romanesco.Model.States {
    public class NoneState : Interface.IFieldState {
        public ReactiveProperty<string> Title { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<object> Content { get; } = new ReactiveProperty<object>();
        public ReactiveProperty<string> FormattedString { get; } = new ReactiveProperty<string>();
        public Type Type => typeof(object);
    }
}
