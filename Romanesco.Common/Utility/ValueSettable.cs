using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Romanesco.Common.Utility
{
    public class ValueSettability
    {
        private Action<object, object> setter;

        public Type Type { get; }
        public string MemberName { get; }

        public ValueSettability(PropertyInfo property)
        {
            setter = (subject, value) => property.SetValue(subject, value);
            Type = property.PropertyType;
            MemberName = property.Name;
        }

        public ValueSettability(FieldInfo field)
        {
            setter = (subject, value) => field.SetValue(subject, value);
            Type = field.FieldType;
            MemberName = field.Name;
        }

        public void SetValue(object subject, object value)
        {
            setter.Invoke(subject, value);
        }

        public void RegisterContentProperty(object subject, IObservable<object> content)
        {
            content.Subscribe(value => setter(subject, value));
        }
    }
}
