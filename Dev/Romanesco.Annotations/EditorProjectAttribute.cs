using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Annotations
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public class EditorProjectAttribute : Attribute
    {
    }
}
