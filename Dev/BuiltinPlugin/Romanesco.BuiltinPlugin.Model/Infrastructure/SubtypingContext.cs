using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.BuiltinPlugin.Model.Infrastructure
{
	public class SubtypingContext
	{
		private readonly Dictionary<Type, SubtypingList> table = new Dictionary<Type, SubtypingList>();

		public void Add(Type baseType, SubtypingList list)
		{
			table[baseType] = list;
		}

		public SubtypingList? Get(Type baseType)
		{
			return table.TryGetValue(baseType, out var list) ? list : null;
		}
	}
}
