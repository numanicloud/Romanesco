using Reactive.Bindings;
using Romanesco.Common;
using Romanesco.Common.Utility;
using Romanesco.Model.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
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
        private readonly StateInterpretFunc interpret;
        private readonly CommandHistory history;
        private readonly IList listInstance;

        public ReactiveProperty<string> Title { get; }
        public IReadOnlyReactiveProperty<string> FormattedString { get; } = new ReactiveProperty<string>();
        public Type Type => Storage.Type;
        public Type ElementType { get; }
        public ValueStorage Storage { get; }
        public ReadOnlyReactiveCollection<IFieldState> Elements { get; }
        public IObservable<Exception> OnError => Observable.Never<Exception>();
        public IObservable<Unit> OnEdited => onContentsChanged;

        public ListState(ValueStorage storage, StateInterpretFunc interpret, CommandHistory history)
        {
            Title = new ReactiveProperty<string>(storage.MemberName);
            elementsMutable = new ReactiveCollection<(IFieldState state, IDisposable disposable)>();
            Elements = elementsMutable.ToReadOnlyReactiveCollection(x => x.state);
            Storage = storage;
            this.interpret = interpret;
            this.history = history;

            ElementType = Storage.Type.GetGenericArguments()[0];

            FormattedString = onContentsChanged.Select(_ => $"Count = {elementsMutable.Count}")
                .ToReactiveProperty("Count = 0");

            // 初期値を読み込み。初期値が無かったら生成
            listInstance = Storage.GetValue() as IList;
            if (listInstance == null)
            {
                listInstance = Activator.CreateInstance(typeof(List<>).MakeGenericType(ElementType)) as IList;
            }
            else
            {
                LoadInitialValue(listInstance, interpret);
            }
        }

        private void LoadInitialValue(IList loadedListInstance, StateInterpretFunc interpret)
        {
            int index = 0;
            foreach (var item in loadedListInstance)
            {
                var storage = new ValueStorage(
                    ElementType,
                    $"{index}",
                    (value, oldValue) =>
                    {
                        var index = loadedListInstance.IndexOf(oldValue);
                        loadedListInstance[index] = value;
                    },
                    item);
                var state = interpret(storage);

                var disposable = SubscribeElementState(state);
                elementsMutable.Add((state, disposable));
            }
        }

        public IFieldState AddNewElement()
        {
            var index = elementsMutable.Count;
            listInstance.Add(GetDefaultValue());

            var settability = new ValueStorage(
                ElementType,
                $"{index}",
                (value, oldValue) =>
                {
                    var index = listInstance.IndexOf(oldValue);
                    listInstance[index] = value;
                },
                listInstance[index]);
            var state = interpret(settability);

            var disposable = SubscribeElementState(state);
            elementsMutable.Add((state, disposable));

            onContentsChanged.OnNext(Unit.Default);

            if (!history.IsOperating)
            {
                var memento = new AddElementToListCommandMemento(this, state, index);
                history.PushMemento(memento);
            }

            return state;
        }

        private object GetDefaultValue()
        {
            if (ElementType.IsValueType)
            {
                return Activator.CreateInstance(ElementType);
            }

            return null;
        }

        private IDisposable SubscribeElementState(IFieldState state)
        {
            return state.OnEdited
                .Where(value => value != null)
                .Subscribe(value => onContentsChanged.OnNext(Unit.Default));
        }

        public void Insert(IFieldState state, int index)
        {
            listInstance.Insert(index, state.Storage.GetValue());

            var disposable = SubscribeElementState(state);
            elementsMutable.Insert(index, (state, disposable));

            onContentsChanged.OnNext(Unit.Default);

            if (!history.IsOperating)
            {
                var memento = new AddElementToListCommandMemento(this, state, index);
                history.PushMemento(memento);
            }
        }

        public void RemoveAt(int index)
        {
            var state = elementsMutable[index].state;

            elementsMutable[index].disposable.Dispose();
            elementsMutable.RemoveAt(index);
            listInstance.RemoveAt(index);
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
