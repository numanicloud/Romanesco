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
        public Type Type => Storage.Type;
        public ValueStorage Storage { get; }
        public Common.IFieldState[] Fields { get; }
        public IObservable<Exception> OnError => onErrorSubject;
        public IObservable<Unit> OnEdited { get; }

        public ClassState(ValueStorage storage, Common.IFieldState[] fields)
        {
            Title = new ReactiveProperty<string>(storage.MemberName);
            Storage = storage;
            Fields = fields;

            OnEdited = Observable.Merge(fields.Select(x => x.OnEdited));

            FormattedString = OnEdited.Select(_ =>
            {
                try
                {
                    return Storage.GetValue().ToString();
                }
                catch (Exception ex)
                {
                    onErrorSubject.OnNext(ContentAccessException.GetFormattedStringError(ex));
                    return "";
                }
            }).ToReadOnlyReactiveProperty(storage.GetValue().ToString());
        }
    }
}
