﻿using Reactive.Bindings;
using Romanesco.Common.ViewModel.Interfaces;
using System;
using System.Reactive;
using System.Reactive.Linq;
using Romanesco.Common.Model.Implementations;

namespace Romanesco.ViewModel.States
{
    public class NoneViewModel : IStateViewModel
    {
        private readonly NoneState state;

        public IReadOnlyReactiveProperty<string> Title => state.Title;

        public IReadOnlyReactiveProperty<string> FormattedString => state.FormattedString;

        public IObservable<Unit> ShowDetail => Observable.Never<Unit>();

        public IObservable<Exception> OnError => state.OnError;

        public NoneViewModel(NoneState state)
        {
            this.state = state;
        }
    }
}
