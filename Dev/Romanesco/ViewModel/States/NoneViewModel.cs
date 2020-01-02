﻿using Reactive.Bindings;
using Romanesco.Common;
using Romanesco.Model.States;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;

namespace Romanesco.ViewModel {
    public class NoneViewModel : IStateViewModel {
        private readonly NoneState state;

        public ReactiveProperty<string> Title { get; }

        public ReactiveProperty<object> Content { get; }

        public ReactiveProperty<string> FormattedString { get; }

        public IObservable<Unit> ShowDetail { get; }

        public IObservable<Exception> OnError => state.OnError;

        public NoneViewModel(NoneState state) {
            this.state = state;
            Title = state.Title;
            Content = state.Content;
            FormattedString = state.FormattedString;
            ShowDetail = Observable.Never<Unit>();
        }
    }
}