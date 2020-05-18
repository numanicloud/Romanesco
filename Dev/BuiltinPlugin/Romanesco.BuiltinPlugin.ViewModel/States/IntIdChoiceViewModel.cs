using Reactive.Bindings;
using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;

namespace Romanesco.ViewModel.States
{
    public class IntIdChoiceViewModel : IStateViewModel
    {
        private readonly IntIdChoiceState state;

        public IReadOnlyReactiveProperty<string> Title => state.Title;

        public IReadOnlyReactiveProperty<string> FormattedString => state.FormattedString;

        public IObservable<Unit> ShowDetail => Observable.Never<Unit>();

        public IObservable<Exception> OnError => state.OnError;

        public ObservableCollection<IFieldState>? Choices { get; private set; }

        public ReactiveProperty<IFieldState> SelectedItem => state.SelectedItem;

        public IntIdChoiceViewModel(IntIdChoiceState state)
        {
            this.state = state;
            state.Master.Subscribe(list =>
            {
                if (list != null)
                {
                    Choices = list.State.Elements;
                }
            });
        }
    }
}
