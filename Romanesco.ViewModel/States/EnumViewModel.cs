using Reactive.Bindings;
using Romanesco.Model.States;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;

namespace Romanesco.ViewModel.States
{
    public class EnumViewModel : Common.IStateViewModel
    {
        private readonly EnumState state;

        public ReactiveProperty<string> Title => state.Title;

        public ReactiveProperty<object> Content => state.Content;

        public ReactiveProperty<string> FormattedString => state.FormattedString;

        public IObservable<Unit> ShowDetail => Observable.Never<Unit>();

        public IObservable<Exception> OnError => state.OnError;

        public object[] Choices => state.Choices;

        public EnumViewModel(EnumState state)
        {
            this.state = state;
        }
    }
}
