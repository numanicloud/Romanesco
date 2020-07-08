using System;

namespace Romanesco.Annotations
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public class EditorProjectAttribute : Attribute
    {
    }
}
