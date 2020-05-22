using Reactive.Bindings;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Exceptions;
using Romanesco.Common.Model.Interfaces;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Romanesco.Common.Model.Implementations
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

            // 初期値を読み込み、変更を反映する処理を登録
            // TODO: !演算子を使わないためにも、class版とstruct版のPrimitiveTypeStateが必要
            PrimitiveContent.Value = (T)Storage.GetValue()!;

            PrimitiveContent.Subscribe(value =>
            {
                Storage.SetValue(value);
            });

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
            }).ToReadOnlyReactiveProperty(SelectFormattedString(PrimitiveContent.Value));

            // Undo/Redo登録
            Storage.OnValueChangedWithOldValue
                .Where(_ => !history.IsOperating)
                .Select(t => new ContentEditCommandMemento(x => PrimitiveContent.Value = (T)x!, t.old, t.value))
                .Subscribe(history.PushMemento);
        }

        protected virtual string SelectFormattedString(T value)
        {
            return value?.ToString() ?? "";
        }
    }
}
