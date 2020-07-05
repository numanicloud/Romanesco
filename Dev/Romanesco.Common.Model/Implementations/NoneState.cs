using Reactive.Bindings;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using System;
using System.Reactive;
using System.Reactive.Linq;

namespace Romanesco.Model.States
{
    public class NoneState : IFieldState
    {
        public ReactiveProperty<string> Title { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<object> Content { get; } = new ReactiveProperty<object>();
        public IReadOnlyReactiveProperty<string> FormattedString { get; } = new ReactiveProperty<string>();
        public Type Type => typeof(object);
        public ValueStorage Storage { get; } = new ValueStorage(typeof(int), "None", (value, args) => { }, 0);
        public IObservable<Exception> OnError => Observable.Never<Exception>();
        public IObservable<Unit> OnEdited => Observable.Never<Unit>();

        public void Dispose()
        {
	        Title.Dispose();
	        Content.Dispose();
	        FormattedString.Dispose();
        }
    }
}
