using Reactive.Bindings;
using Romanesco.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;

namespace Romanesco.Model.States {
    public class ClassState : Common.IFieldState
    {
        public ReactiveProperty<object> Content { get; } = new ReactiveProperty<object>();
        public ReactiveProperty<string> Title { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> FormattedString { get; } = new ReactiveProperty<string>();
        public Type Type => Settability.Type;
        public ValueSettability Settability { get; }
        public Common.IFieldState[] Fields { get; }

        public ClassState(ValueSettability settability, Romanesco.Common.IFieldState[] fields)
        {
            Title.Value = settability.MemberName;
            Settability = settability;
            Fields = fields;

            Content.Value = Activator.CreateInstance(settability.Type);
            FormattedString = Observable.Merge(fields.Select(x => x.Content))
                .Select(_ => Content.Value.ToString())
                .ToReactiveProperty();

            foreach (var field in fields)
            {
                field.Content.Where(_ => Content.Value != null)
                    .Subscribe(x =>
                {
                    // NoneStateが来るかもしれないので null チェックしながら呼び出す
                    field.Settability?.SetValue(Content.Value, x);
                });
            }
        }
    }
}
