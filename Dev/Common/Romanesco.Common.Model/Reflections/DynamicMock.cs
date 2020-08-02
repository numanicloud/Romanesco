using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Romanesco.Common.Model.Reflections
{
	public class DynamicMock : DynamicObject
	{
		public PropertyInfo[] Properties { get; }
		public FieldInfo[] Fields { get; }
		private readonly IDictionary<string, object?> members;
		private readonly IDictionary<string, CustomAttributeData[]> attributes;

		public string[] Keys => members.Keys.Where(x => !x.StartsWith("Type:")).ToArray();

		public DynamicMock(PropertyInfo[] properties, FieldInfo[] fields)
		{
			Properties = properties;
			Fields = fields;

			object? GetDefault(Type type)
			{
				if (!type.Assembly.ReflectionOnly && type.IsValueType)
				{
					return Activator.CreateInstance(type);
				}
				return null;
			}

			var p = properties.Where(x => x.CanWrite && x.CanRead)
				.Select(x => new
			{
				x.Name, Type = x.PropertyType,
				Attr = x.CustomAttributes
			});
			var m = fields.Where(x => x.IsPublic)
				.Select(x => new
			{
				x.Name, Type = x.FieldType,
				Attr = x.CustomAttributes
			})
				.Concat(p);

			var dic = new Dictionary<string, object?>();
			var attrs = new Dictionary<string, CustomAttributeData[]>();
			foreach (var item in m)
			{
				dic[item.Name] = GetDefault(item.Type);
				dic["Type:" + item.Name] = item.Type;
				attrs[item.Name] = item.Attr.ToArray();
			}

			members = dic;
			attributes = attrs;
		}

		public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
		{
			if (indexes[0] is string key && members.ContainsKey(key))
			{
				members[key] = value;
				return true;
			}
			return false;
		}

		public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object? result)
		{
			if (indexes[0] is string key && members.TryGetValue(key, out result!))
			{
				return true;
			}
			result = null;
			return false;
		}

		public CustomAttributeData? GetAttributeData<TAttribute>(string memberName)
		{
			return attributes[memberName].FirstOrDefault(x =>
			{
				try
				{
					return x.AttributeType.Name == typeof(TAttribute).Name;
				}
				catch (FileNotFoundException)
				{
					// TODO: 一部の属性が読み込まれなかったことは警告に出したい
					Debug.WriteLine($"{nameof(DynamicMock)}.{nameof(GetAttributeData)}:属性が読み込まれませんでした");
					return false;
				}
			});
		}
	}
}
