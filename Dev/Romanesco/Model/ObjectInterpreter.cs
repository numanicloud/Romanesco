using Romanesco.Common;
using Romanesco.Common.Utility;
using Romanesco.Model.States;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;

namespace Romanesco.Model
{
    public class ObjectInterpreter
    {
        public IStateFactory[] Factories { get; }

        public ObjectInterpreter(IStateFactory[] factories)
        {
            Factories = factories;
        }

        public IFieldState InterpretRootAsState(object projectObject, PropertyInfo property)
        {
            return InterpretAsState(new ValueStorage(projectObject, property));
        }

        public IFieldState InterpretRootAsState(object projectObject, FieldInfo field)
        {
            return InterpretAsState(new ValueStorage(projectObject, field));
        }

        private IFieldState InterpretAsState(ValueStorage settability)
        {
            foreach (var factory in Factories)
            {
                var result = factory.InterpretAsState(settability, InterpretAsState);
                if (result != null)
                {
                    return result;
                }
            }
            return new NoneState();
        }
    }
}
