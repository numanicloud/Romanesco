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

        public ReactiveProperty<string> FormattedString { get; }

        public Type Type { get; }

        public ValueSettability Settability { get; }

        public IObservable<Exception> OnError => onError_;

        public object[] Choices { get; }

        public EnumState(ValueSettability settability, CommandHistory history)
        {
            Type = settability.Type;
            Title.Value = settability.MemberName;
            Settability = settability;

            FormattedString = Content.Select(x => x.ToString()).ToReactiveProperty();

            var values = new List<object>();
            foreach (var value in Enum.GetValues(Type))
            {
                values.Add(value);
            }
            Choices = values.ToArray();

            if (Choices.Length == 0)
            {
                onError_.OnNext(new Exception($"列挙体 {settability.Type.Name} に属する列挙子が 0 個でした。"));
            }
            else
            {
                Content.Value = Choices[0];
            }

            Content.Zip(Content.Skip(1), (a, b) => (a, b))
                .Where(_ => !history.IsOperating)
                .Select(t => new ContentEditCommandMemento(x => Content.Value = x, t.a, t.b))
                .Subscribe(history.PushMemento);
        }
    }
}
