using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Annotations
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class EditorSubtypingBaseAttribute : Attribute
	{
	}
}
