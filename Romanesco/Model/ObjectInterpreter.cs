using Romanesco.Common;
using Romanesco.Common.Utility;
using Romanesco.Model.States;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Romanesco.Model
{
    public class ObjectInterpreter
    {
        public ObjectInterpreter(IStateFactory[] factories)
        {
            Factories = factories;
        }

        public IStateFactory[] Factories { get; }

        public IFieldState InterpretAsState(PropertyInfo property)
        {
            return InterpretAsState(new ValueSettability(property));
        }

        public IFieldState InterpretAsState(FieldInfo field)
        {
            return InterpretAsState(new ValueSettability(field));
        }

        private IFieldState InterpretAsState(ValueSettability settability)
        {
            foreach (var factory in Factories)
            {
                var result = factory.InterpretAsState(settability, InterpretAsState, InterpretAsState);
                if (result != null)
                {
                    return result;
                }
            }
            return new NoneState();
        }
    }
}
