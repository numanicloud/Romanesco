﻿using System;
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

        public ReactiveProperty<string> Title { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<object> Content { get; }
        public ReactiveProperty<string> FormattedString { get; } = new ReactiveProperty<string>();
        public Type Type => typeof(T);
        public ReactiveProperty<T> PrimitiveContent { get; } = new ReactiveProperty<T>();
        public ValueSettability Settability { get; }
        public IObservable<Exception> OnError => onErrorSubject;

        public PrimitiveTypeState(ValueSettability settability, CommandHistory history)
        {
            if (settability.Type != Type)
            {
                throw new ArgumentException($"一致しない型のフィールドが渡されました。Expected: {Type.FullName}, Actual: {Settability.Type.FullName}",
                    nameof(settability));
            }

            Title.Value = settability.MemberName;
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
            }).ToReactiveProperty();
            Content = PrimitiveContent.Select(x => (object)x).ToReactiveProperty();
            Settability = settability;

            PrimitiveContent.Zip(PrimitiveContent.Skip(1), (a, b) => (a, b))
                .Where(_ => !history.IsOperating)
                .Select(t => new ContentEditCommandMemento(x => PrimitiveContent.Value = (T)x, t.a, t.b))
                .Subscribe(history.PushMemento);
        }

        protected virtual string SelectFormattedString(T value)
        {
            return value.ToString();
        }
    }
}