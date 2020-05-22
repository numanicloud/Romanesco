using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Annotations
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class EditorSubtypeNameAttribute : Attribute
	{
		public EditorSubtypeNameAttribute(string optionName)
		{
			OptionName = optionName;
		}

		public string OptionName { get; }
	}
}
