﻿using Reactive.Bindings;
using Romanesco.Common.ViewModel.Interfaces;
using Romanesco.Model.States;
using System;
using System.Reactive;
using System.Reactive.Linq;

namespace Romanesco.ViewModel.States
{
    public class EnumViewModel : IStateViewModel
    {
        private readonly EnumState state;

        public IReadOnlyReactiveProperty<string> Title => state.Title;
    
        public IReadOnlyReactiveProperty<string> FormattedString => state.FormattedString;

        public ReactiveProperty<object> Content => state.Content;

        public IObservable<Unit> ShowDetail => Observable.Never<Unit>();

        public IObservable<Exception> OnError => state.OnError;

        public object[] Choices => state.Choices;

        public EnumViewModel(EnumState state)
        {
            this.state = state;
        }
    }
}
