using Reactive.Bindings;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Implementations;
using Romanesco.Common.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Reactive.Bindings.Extensions;

namespace Romanesco.BuiltinPlugin.Model.States
{
    public class EnumState : IFieldState
    {
        private readonly Subject<Exception> onError_ = new Subject<Exception>();
        private readonly List<IDisposable> disposables = new List<IDisposable>();

        public ReactiveProperty<string> Title { get; }

        public IReadOnlyReactiveProperty<string> FormattedString { get; }

        public ReactiveProperty<object> Content { get; }

        public Type Type { get; }

        public ValueStorage Storage { get; }

        public IObservable<Exception> OnError => onError_;

        public IObservable<Unit> OnEdited => Storage.OnValueChanged.Select(x => Unit.Default);

        public object[] Choices { get; }

        public EnumState(ValueStorage storage, CommandHistory history)
        {
            Title = new ReactiveProperty<string>(storage.MemberName);
            Type = storage.Type;
            Storage = storage;

            // Enum変数は初期値がnullでないので !演算子で済ませる
            Content = new ReactiveProperty<object>(storage.GetValue()!);
            Content.Subscribe(x => Storage.SetValue(x))
	            .AddTo(disposables);

            FormattedString = storage.OnValueChanged
                .Select(x => x?.ToString() ?? "")
                .ToReactiveProperty(Content.Value.ToString() ?? "");

            var values = new List<object>();
            foreach (var value in Enum.GetValues(Type))
            {
                if (value != null)
                {
                    values.Add(value);
                }
            }
            Choices = values.ToArray();

            if (Choices.Length == 0)
            {
                onError_.OnNext(new Exception($"列挙体 {storage.Type.Name} に属する列挙子が 0 個でした。"));
                return;
            }

            // Undo/Redo登録
            storage.OnValueChangedWithOldValue
                .Where(_ => !history.IsOperating)
                .Select(t => new ContentEditCommandMemento(x => Storage.SetValue(x), t.old, t.value))
                .Subscribe(history.PushMemento)
                .AddTo(disposables);
        }

        public void Dispose()
        {
            Title.Dispose();
            FormattedString.Dispose();
            Content.Dispose();
            foreach (var disposable in disposables)
            {
	            disposable.Dispose();
            }
        }
    }
}
