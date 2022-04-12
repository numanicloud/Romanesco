using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Common.Model.ValueStorages
{
	public interface IValueStorage
	{
		Type Type { get; }
		string MemberName { get; }
		Attribute[] Attributes { get; }

		object? GetValue();
		void SetValue(object? value);
	}
}
