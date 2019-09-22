using Reactive.Bindings;
using Romanesco.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using System.Text;

namespace Romanesco.Model.States {
    public class ClassState : Common.IFieldState
    {
        private readonly Subject<Unit> onFieldsUpdated = new Subject<Unit>();
        private readonly Subject<Exception> onErrorSubject = new Subject<Exception>();

        public ReactiveProperty<object> Content { get; } = new ReactiveProperty<object>();
        public ReactiveProperty<string> Title { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> FormattedString { get; } = new ReactiveProperty<string>();
        public Type Type => Settability.Type;
        public ValueSettability Settability { get; }
        public Common.IFieldState[] Fields { get; }
        public IObservable<Exception> OnError => onErrorSubject;

        public ClassState(ValueSettability settability, Romanesco.Common.IFieldState[] fields)
        {
            Title.Value = settability.MemberName;
            Settability = settability;
            Fields = fields;

            Content.Value = Activator.CreateInstance(settability.Type);
            FormattedString = onFieldsUpdated.Select(_ =>
            {
                try
                {
                    return Content.Value.ToString();
                }
                catch (Exception ex)
                {
                    onErrorSubject.OnNext(ContentAccessException.GetFormattedStringError(ex));
                    return "";
                }
            }).ToReactiveProperty();

            foreach (var field in fields)
            {
                field.Content.Where(_ => Content.Value != null)
                    .Subscribe(x =>
                {
                    try
                    {
                        // NoneStateが来るかもしれないので null チェックしながら呼び出す
                        field.Settability?.SetValue(Content.Value, x);
                    }
                    catch (Exception ex)
                    {
                        onErrorSubject.OnNext(ContentAccessException.GetSetterError(ex));
                    }
                    onFieldsUpdated.OnNext(Unit.Default);
                });
            }
        }
    }
}
