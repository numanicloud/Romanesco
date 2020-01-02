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
        private readonly Subject<Exception> onErrorSubject = new Subject<Exception>();

        public ReactiveProperty<string> Title { get; } = new ReactiveProperty<string>();
        public IReadOnlyReactiveProperty<string> FormattedString { get; } = new ReactiveProperty<string>();
        public Type Type => Settability.Type;
        public ValueSettability Settability { get; }
        public Common.IFieldState[] Fields { get; }
        public IObservable<Exception> OnError => onErrorSubject;
        public IObservable<Unit> OnEdited { get; }

        public ClassState(ValueSettability settability, Common.IFieldState[] fields)
        {
            Title = new ReactiveProperty<string>(settability.MemberName);
            Settability = settability;
            Fields = fields;

            OnEdited = Observable.Merge(fields.Select(x => x.OnEdited));

            FormattedString = OnEdited.Select(_ =>
            {
                try
                {
                    return Settability.GetValue().ToString();
                }
                catch (Exception ex)
                {
                    onErrorSubject.OnNext(ContentAccessException.GetFormattedStringError(ex));
                    return "";
                }
            }).ToReadOnlyReactiveProperty(settability.GetValue().ToString());
        }
    }
}
