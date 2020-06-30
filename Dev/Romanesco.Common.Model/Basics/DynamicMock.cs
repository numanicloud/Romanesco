using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Romanesco.Common.Model.Basics
{
	public class DynamicMock : DynamicObject
	{
		private readonly IDictionary<string, object?> members;

		public string[] Keys => members.Keys.Where(x => !x.StartsWith("Type:")).ToArray();

		public DynamicMock(PropertyInfo[] properties, FieldInfo[] fields)
		{
			object? GetDefault(Type type)
			{
				if (type.IsValueType)
				{
					return Activator.CreateInstance(type);
				}
				else
				{
					return null;
				}
			}

			// TODO: EditorMemberAttribute の Order によって並べ替える
			var p = properties.Select(x => new { x.Name, Type = x.PropertyType });
			var m = fields.Select(x => new { x.Name, Type = x.FieldType })
				.Concat(p);

			var dic = new Dictionary<string, object?>();
			foreach (var item in m)
			{
				dic[item.Name] = GetDefault(item.Type);
				dic["Type:" + item.Name] = item.Type;
			}

			members = dic;
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
	}
}
