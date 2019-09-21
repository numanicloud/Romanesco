using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using Reactive.Bindings;
using Romanesco.Common;
using Romanesco.Common.Utility;

namespace Romanesco.Model.States
{
    public class ListState : IFieldState
    {
        private readonly Subject<Unit> onContentsChanged = new Subject<Unit>();
        private readonly ReactiveCollection<(IFieldState state, IDisposable disposable)> elementsMutable;
        private readonly Type elementType;
        private readonly StateInterpretFunc interpret;

        public ReactiveProperty<string> Title { get; } = new ReactiveProperty<string>();

        public ReactiveProperty<object> Content { get; } = new ReactiveProperty<object>();

        public ReactiveProperty<string> FormattedString { get; } = new ReactiveProperty<string>();

        public Type Type => Settability.Type;

        public ValueSettability Settability { get; }

        public ReactiveProperty<IList> ArrayContent { get; } = new ReactiveProperty<IList>();

        public ReadOnlyReactiveCollection<IFieldState> Elements { get; }

        public ListState(ValueSettability settability, StateInterpretFunc interpret)
        {
            elementsMutable = new ReactiveCollection<(IFieldState state, IDisposable disposable)>();
            Elements = elementsMutable.ToReadOnlyReactiveCollection(x => x.state);
            Settability = settability;
            this.interpret = interpret;
            Title.Value = settability.MemberName;
            FormattedString = onContentsChanged.Select(_ => $"Count = {elementsMutable.Count}").ToReactiveProperty("Count = 0");

            elementType = Settability.Type.GetGenericArguments()[0];
            ArrayContent.Value = Activator.CreateInstance(settability.Type) as IList;
            Content = ArrayContent.Select(x => (object)x).ToReactiveProperty();
        }

        public IFieldState AddNewElement()
        {
            var state = interpret(new ValueSettability(
                elementType,
                $"Element of {Title.Value}",
                (subject, value, index) =>
                {
                    var array = subject as IList;
                    array[(int)index[0]] = value;
                }));

            var disposable = state.Content
                .Skip(1)
                .Where(value => value != null)
                .Subscribe(value =>
            {
                var index = elementsMutable.IndexOf(x => x.state == state);
                state.Settability?.SetValue(ArrayContent.Value, value, new object[] { index });
                onContentsChanged.OnNext(Unit.Default);
            });
            elementsMutable.Add((state, disposable));
            onContentsChanged.OnNext(Unit.Default);

            ArrayContent.Value = elementsMutable.Select(x => x.state.Content.Value).ToList();

            return state;
        }

        public void RemoveAt(int index)
        {
            var entry = elementsMutable[index];
            entry.disposable.Dispose();
            elementsMutable.RemoveAt(index);
            onContentsChanged.OnNext(Unit.Default);

            ArrayContent.Value = elementsMutable.Select(x => x.state.Content.Value).ToList();
        }
    }
}
