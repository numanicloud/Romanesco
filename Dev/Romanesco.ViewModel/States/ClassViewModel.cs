using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Subjects;
using System.Text;
using Reactive.Bindings;
using Romanesco.Common;
using Romanesco.Model.States;

namespace Romanesco.ViewModel.States
{
    public class ClassViewModel : IStateViewModel
    {
        private readonly ClassState state;
        private readonly Subject<Unit> showDetailSubject = new Subject<Unit>();

        public IReadOnlyReactiveProperty<string> Title => state.Title;
        public IReadOnlyReactiveProperty<string> FormattedString => state.FormattedString;
        public IStateViewModel[] Fields { get; }
        public ReactiveCommand EditCommand { get; }
        public IObservable<Unit> ShowDetail => showDetailSubject;
        public IObservable<Exception> OnError => state.OnError;

        public ClassViewModel(ClassState state, IStateViewModel[] fields)
        {
            this.state = state;
            Fields = fields;
            EditCommand = new ReactiveCommand();
            EditCommand.Subscribe(_ => showDetailSubject.OnNext(Unit.Default));
        }
    }
}
