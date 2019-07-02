using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;

namespace Romanesco.Model.States {
    public class ClassState {
        public ReactiveProperty<object> Content { get; } = new ReactiveProperty<object>();
        public ReactiveProperty<string> Title { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> FormattedString { get; } = new ReactiveProperty<string>();

        public ClassState(object content) {
            Content.Value = content;
            FormattedString = Content.Select(x => x.ToString())
                .ToReactiveProperty(content.ToString());
        }
    }
}
