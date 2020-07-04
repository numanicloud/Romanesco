using System;

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
