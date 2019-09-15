using Romanesco.Common.Utility;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Romanesco.Common
{

    public delegate IFieldState StateInterpretFunc(ValueSettability settability);
    public delegate IFieldState InterpretProperty(PropertyInfo propertyInfo);
    public delegate IFieldState InterpretField(FieldInfo fieldInfo);

    public interface IStateFactory
    {
        IFieldState InterpretAsState(ValueSettability settability, StateInterpretFunc interpret);
    }
}
