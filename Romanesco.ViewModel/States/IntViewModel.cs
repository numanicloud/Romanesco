using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using Reactive.Bindings;
using Romanesco.Model.States;

namespace Romanesco.ViewModel.States {
    public class IntViewModel {
        public IntViewModel(IntState state) {
            State = state;

            FormattedString = State.Content.Select(x => x.ToString() + "yen")
                .ToReactiveProperty(State.Content.Value + "yen");
        }

        public IntState State { get; }
        public ReactiveProperty<string> FormattedString { get; }
    }
}
