using System;
using System.Collections;
using System.Reflection;

namespace Romanesco.Common.Model.Basics
{
	public class ValueStorageFactory
	{
		public ValueStorage FromField(object subject, FieldInfo field)
		{
			return new ValueStorage(subject, field);
		}

		public ValueStorage FromProperty(object subject, PropertyInfo property)
		{
			return new ValueStorage(subject, property);
		}

		public ValueStorage FromListElement(Type elementType, IList list, int index, object? initialValue)
		{
			return new ValueStorage(
				elementType,
				$"{index}",
				(value, oldValue) =>
				{
					var index = list.IndexOf(oldValue);
					list[index] = value;
				}, list[index] ?? throw new IndexOutOfRangeException());
		}

		public ValueStorage FromDynamicMocksMember(dynamic subject, string key)
		{
			Type type = (Type)subject["Type:" + key];
			SetterFunction setter = (value, oldValue) => subject[key] = value;
			return new ValueStorage(type, key, setter, subject[key]);
		}
	}
}
