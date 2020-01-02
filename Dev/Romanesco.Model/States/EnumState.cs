using Reactive.Bindings;
using Romanesco.Common;
using Romanesco.Common.Utility;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;

namespace Romanesco.Model.States
{
    public class EnumState : IFieldState
    {
        private readonly Subject<Exception> onError_ = new Subject<Exception>();

        public ReactiveProperty<string> Title { get; } = new ReactiveProperty<string>();

        public ReactiveProperty<object> Content { get; } = new ReactiveProperty<object>();

        public IReadOnlyReactiveProperty<string> FormattedString { get; }

        public Type Type { get; }

        public ValueStorage Storage { get; }

        public IObservable<Exception> OnError => onError_;

        public object[] Choices { get; }

        public IObservable<Unit> OnEdited => Storage.OnValueChanged.Select(x => Unit.Default);

        public EnumState(ValueStorage storage, CommandHistory history)
        {
            Title = new ReactiveProperty<string>(storage.MemberName);
            Type = storage.Type;
            Storage = storage;

            FormattedString = storage.OnValueChanged
                .Select(x => x.ToString())
                .ToReactiveProperty();

            var values = new List<object>();
            foreach (var value in Enum.GetValues(Type))
            {
                values.Add(value);
            }
            Choices = values.ToArray();

            if (Choices.Length == 0)
            {
                onError_.OnNext(new Exception($"列挙体 {storage.Type.Name} に属する列挙子が 0 個でした。"));
            }
            else
            {
                Storage.SetValue(Choices[0]);
            }

            storage.OnValueChangedWithOldValue
                .Where(_ => !history.IsOperating)
                .Select(t => new ContentEditCommandMemento(x => Content.Value = x, t.old, t.value))
                .Subscribe(history.PushMemento);
        }
    }
}
