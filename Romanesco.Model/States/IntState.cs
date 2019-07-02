using System;
using System.Collections.Generic;
using System.Text;
using Reactive.Bindings;

namespace Romanesco.Model.States {
    public class IntState {
        public ReactiveProperty<int> Content { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<string> Title { get; } = new ReactiveProperty<string>();

        public IntState(int value, string title) {
            Content.Value = value;
            Title.Value = title;
        }
    }
}
