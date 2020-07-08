using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using System.Text;
using Reactive.Bindings;
using Romanesco.Common.Utility;

namespace Romanesco.Common
{
    public class PrimitiveTypeState<T> : IFieldState
    {
        private readonly Subject<Exception> onErrorSubject = new Subject<Exception>();

        public ReactiveProperty<string> Title { get; }
        public IReadOnlyReactiveProperty<string> FormattedString { get; }
        public Type Type => typeof(T);
        public ReactiveProperty<T> PrimitiveContent { get; } = new ReactiveProperty<T>();
        public ValueStorage Storage { get; }
        public IObservable<Exception> OnError => onErrorSubject;
        public IObservable<Unit> OnEdited => Storage.OnValueChanged.Select(x => Unit.Default);

        public PrimitiveTypeState(ValueStorage storage, CommandHistory history)
        {
            if (storage.Type != Type)
            {
                throw new ArgumentException($"一致しない型のフィールドが渡されました。Expected: {Type.FullName}, Actual: {Storage.Type.FullName}",
                    nameof(storage));
            }

            Storage = storage;
            Title = new ReactiveProperty<string>(storage.MemberName);
            
            FormattedString = PrimitiveContent.Select(x =>
            {
                try
                {
                    return SelectFormattedString(x);
                }
                catch (Exception ex)
                {
                    onErrorSubject.OnNext(ContentAccessException.GetFormattedStringError(ex));
                    return "";
                }
            }).ToReadOnlyReactiveProperty();

            // Undo/Redo登録
            Storage.OnValueChangedWithOldValue
                .Where(_ => !history.IsOperating)
                .Select(t => new ContentEditCommandMemento(x => PrimitiveContent.Value = (T)x, t.old, t.value))
                .Subscribe(history.PushMemento);

            // 初期値を読み込み、変更を反映する処理を登録
            PrimitiveContent.Value = (T)Storage.GetValue();
            PrimitiveContent.Subscribe(value => Storage.SetValue(value));
        }

        protected virtual string SelectFormattedString(T value)
        {
            return value.ToString();
        }
    }
}
