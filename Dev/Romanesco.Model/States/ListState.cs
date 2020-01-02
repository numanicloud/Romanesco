using Reactive.Bindings;
using Romanesco.Common;
using Romanesco.Common.Utility;
using Romanesco.Model.Infrastructure;
using System;
using System.Collections;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Romanesco.Model.States
{
    public class ListState : IFieldState
    {
        private readonly Subject<Unit> onContentsChanged = new Subject<Unit>();
        private readonly ReactiveCollection<(IFieldState state, IDisposable disposable)> elementsMutable;
        private readonly Type elementType;
        private readonly StateInterpretFunc interpret;
        private readonly CommandHistory history;

        public ReactiveProperty<string> Title { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<object> Content { get; } = new ReactiveProperty<object>();
        public ReactiveProperty<string> FormattedString { get; } = new ReactiveProperty<string>();
        public Type Type => Settability.Type;
        public Type ElementType => elementType;
        public ValueSettability Settability { get; }
        public ReactiveProperty<IList> ArrayContent { get; } = new ReactiveProperty<IList>();
        public ReadOnlyReactiveCollection<IFieldState> Elements { get; }
        public IObservable<Exception> OnError => Observable.Never<Exception>();

        public ListState(ValueSettability settability, StateInterpretFunc interpret, CommandHistory history)
        {
            elementsMutable = new ReactiveCollection<(IFieldState state, IDisposable disposable)>();
            Elements = elementsMutable.ToReadOnlyReactiveCollection(x => x.state);
            Settability = settability;
            this.interpret = interpret;
            this.history = history;
            Title.Value = settability.MemberName;
            FormattedString = onContentsChanged.Select(_ => $"Count = {elementsMutable.Count}")
                .ToReactiveProperty("Count = 0");

            elementType = Settability.Type.GetGenericArguments()[0];
            ArrayContent.Value = Activator.CreateInstance(settability.Type) as IList;
            Content = onContentsChanged.Select(x => (object)ArrayContent.Value)
                .ToReactiveProperty(ArrayContent.Value, ReactivePropertyMode.RaiseLatestValueOnSubscribe);
            Content.Subscribe(_ => Console.WriteLine("Hoge"), ex => throw ex);
        }

        public IFieldState AddNewElement()
        {
            var index = elementsMutable.Count;
            var state = interpret(new ValueSettability(
                elementType,
                $"{index}",
                (subject, value, index) =>
                {
                    var array = subject as IList;
                    array[(int)index[0]] = value;
                }));
            IDisposable disposable = SubscribeElementState(state);

            elementsMutable.Add((state, disposable));
            ArrayContent.Value.Add(state.Content.Value);
            onContentsChanged.OnNext(Unit.Default);

            if (!history.IsOperating)
            {
                var memento = new AddElementToListCommandMemento(this, state, index);
                history.PushMemento(memento);
            }

            return state;
        }

        private IDisposable SubscribeElementState(IFieldState state)
        {
            return state.Content
                .Skip(1)
                .Where(value => value != null)
                .Subscribe(value =>
                {
                    var index = elementsMutable.IndexOf(x => x.state == state);
                    state.Settability?.SetValue(ArrayContent.Value, value, new object[] { index });
                    onContentsChanged.OnNext(Unit.Default);
                });
        }

        public void Insert(IFieldState state, int index)
        {
            var disposable = SubscribeElementState(state);

            elementsMutable.Insert(index, (state, disposable));
            ArrayContent.Value.Insert(index, state.Content.Value);
            onContentsChanged.OnNext(Unit.Default);
        }

        public void RemoveAt(int index)
        {
            var state = elementsMutable[index].state;

            elementsMutable[index].disposable.Dispose();
            elementsMutable.RemoveAt(index);
            ArrayContent.Value.RemoveAt(index);
            onContentsChanged.OnNext(Unit.Default);

            for (int i = index; i < elementsMutable.Count; i++)
            {
                elementsMutable[i].state.Title.Value = i.ToString();
            }

            if (!history.IsOperating)
            {
                var memento = new RemoveElementToListCommandMemento(this, state, index);
                history.PushMemento(memento);
            }
        }
    }
}
