using Romanesco.Common.Model.Basics;
using System.Reflection;

namespace Romanesco.Common.Model.Interfaces
{

    public delegate IFieldState StateInterpretFunc(ValueStorage settability);
    public delegate IFieldState InterpretProperty(PropertyInfo propertyInfo);
    public delegate IFieldState InterpretField(FieldInfo fieldInfo);

    public interface IStateFactory
    {
        IFieldState InterpretAsState(ValueStorage settability, StateInterpretFunc interpret);
    }
}
