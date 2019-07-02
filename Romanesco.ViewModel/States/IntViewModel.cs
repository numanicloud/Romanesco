﻿using System;
using System.Collections.Generic;
using System.Text;
using Reactive.Bindings;
using Romanesco.Model.States;

namespace Romanesco.ViewModel {
    public class IntViewModel : Interface.IStateViewModel {
        private readonly IntState state;

        public ReactiveProperty<string> Title { get; }

        public ReactiveProperty<object> Content { get; }

        public ReactiveProperty<string> FormattedString { get; }

        public IntViewModel(IntState state) {
            this.state = state;
            Title = state.Title;
            Content = state.Content;
            FormattedString = state.FormattedString;
        }
    }
}
