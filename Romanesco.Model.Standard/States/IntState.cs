using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using Reactive.Bindings;

namespace Romanesco.Model.States {
    public class IntState : IFieldState {
        public ReactiveProperty<int> IntContent { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<string> Title { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<object> Content { get; }
        public ReactiveProperty<string> FormattedString { get; }

        public IntState(int value, string title) {
            Content.Value = value;
            Title.Value = title;
            Content = IntContent.Select(x => (object)x).ToReactiveProperty();
            FormattedString = IntContent.Select(x => x.ToString()).ToReactiveProperty();
        }
    }
}
