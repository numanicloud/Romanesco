using Reactive.Bindings;
using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.Common.ViewModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Subjects;
using Reactive.Bindings.Extensions;

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
        public List<IDisposable> Disposables => state.Disposables;

        public ClassViewModel(ClassState state, IStateViewModel[] fields)
        {
            this.state = state;
            Fields = fields;
            EditCommand = new ReactiveCommand();
            EditCommand.Subscribe(_ => showDetailSubject.OnNext(Unit.Default))
	            .AddTo(Disposables);

            state.Disposables.Add(EditCommand);
        }
    }
}
