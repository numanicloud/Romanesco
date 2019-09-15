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
    public class ArrayState : IFieldState
    {
        private readonly Subject<Unit> onContentsChanged = new Subject<Unit>();
        private readonly ReactiveCollection<IFieldState> elementsMutable;
        private readonly Type elementType;
        private readonly StateInterpretFunc interpret;

        public ReactiveProperty<string> Title { get; } = new ReactiveProperty<string>();

        public ReactiveProperty<object> Content { get; } = new ReactiveProperty<object>();

        public ReactiveProperty<string> FormattedString { get; } = new ReactiveProperty<string>();

        public Type Type => Settability.Type;

        public ValueSettability Settability { get; }

        public ReactiveProperty<Array> ArrayContent { get; } = new ReactiveProperty<Array>();

        public ReadOnlyReactiveCollection<IFieldState> Elements { get; }

        public ArrayState(ValueSettability settability, StateInterpretFunc interpret)
        {
            elementsMutable = new ReactiveCollection<IFieldState>();
            Elements = elementsMutable.ToReadOnlyReactiveCollection();
            Settability = settability;
            this.interpret = interpret;
            Title.Value = settability.MemberName;
            FormattedString = onContentsChanged.Select(_ => $"Length = {Elements.Count}").ToReactiveProperty();

            elementType = Settability.Type.GetElementType();
            ArrayContent.Value = Array.CreateInstance(elementType, 0);
            Content = ArrayContent.Select(x => (object)x).ToReactiveProperty();
        }

        public IFieldState AddNewElement()
        {
            var state = interpret(new ValueSettability(
                elementType,
                $"Element of {Title.Value}",
                (subject, value, index) =>
                {
                    var array = subject as Array;
                    array.SetValue(value, (int)index[0]);
                }));

            state.Content.Skip(1).Where(value => value != null)
                .Subscribe(value =>
            {
                var index = elementsMutable.IndexOf(state);
                state.Settability?.SetValue(ArrayContent.Value, value, new object[] { index });
                onContentsChanged.OnNext(Unit.Default);
            });
            elementsMutable.Add(state);
            onContentsChanged.OnNext(Unit.Default);

            ArrayContent.Value = elementsMutable.Select(x => x.Content.Value).ToArray();

            return state;
        }
    }
}
