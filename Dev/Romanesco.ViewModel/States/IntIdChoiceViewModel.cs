using Reactive.Bindings;
using Romanesco.Common;
using Romanesco.Model.States;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;

namespace Romanesco.ViewModel.States
{
    public class IntIdChoiceViewModel : IStateViewModel
    {
        private readonly IntIdChoiceState state;

        public IReadOnlyReactiveProperty<string> Title => state.Title;

        public IReadOnlyReactiveProperty<string> FormattedString => state.FormattedString;

        public IObservable<Unit> ShowDetail => Observable.Never<Unit>();

        public IObservable<Exception> OnError => state.OnError;

        public ReadOnlyReactiveCollection<IFieldState> Choices { get; private set; }

        public ReactiveProperty<IFieldState> SelectedItem => state.SelectedItem;

        public IntIdChoiceViewModel(IntIdChoiceState state)
        {
            this.state = state;
            state.Master.Where(x => x != null)
                .Subscribe(list => Choices = list.State.Elements);
        }
    }
}
