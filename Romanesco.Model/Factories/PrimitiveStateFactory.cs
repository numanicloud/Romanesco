using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Romanesco.Common;
using Romanesco.Common.Utility;
using Romanesco.Model.States;

namespace Romanesco.Model.Factories
{
    public class PrimitiveStateFactory : IStateFactory
    {
        public IFieldState InterpretAsState(ValueSettability settability,
            InterpretProperty interpretProperty,
            InterpretField interpretField)
        {
            if (settability.Type == typeof(int)) return new IntState(settability);
            if (settability.Type == typeof(float)) return new FloatState(settability);
            if (settability.Type == typeof(string)) return new StringState(settability);
            if (settability.Type == typeof(bool)) return new BoolState(settability);

            if (settability.Type == typeof(byte)) return new ByteState(settability);
            if (settability.Type == typeof(short)) return new ShortState(settability);
            if (settability.Type == typeof(long)) return new LongState(settability);
            if (settability.Type == typeof(double)) return new DoubleState(settability);
            return null;
        }
    }
}
