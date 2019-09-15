using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Romanesco.Common.Utility
{
    public class ValueSettability
    {
        private Action<object, object, object[]> setter;

        public Type Type { get; }
        public string MemberName { get; }

        public ValueSettability(PropertyInfo property)
        {
            setter = (subject, value, index) => property.SetValue(subject, value, index);
            Type = property.PropertyType;
            MemberName = property.Name;
        }

        public ValueSettability(FieldInfo field)
        {
            setter = (subject, value, index) => field.SetValue(subject, value);
            Type = field.FieldType;
            MemberName = field.Name;
        }

        public ValueSettability(Type type, string memberName, Action<object, object, object[]> setter)
        {
            this.setter = setter;
            Type = type;
            MemberName = memberName;
        }

        public void SetValue(object subject, object value, object[] context = null)
        {
            setter.Invoke(subject, value, context);
        }
    }
}
