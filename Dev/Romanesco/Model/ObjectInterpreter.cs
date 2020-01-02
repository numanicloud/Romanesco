﻿using Romanesco.Common;
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

        public IFieldState InterpretAsState(object projectObject, PropertyInfo property)
        {
            return InterpretAsState(new ValueSettability(projectObject, property));
        }

        public IFieldState InterpretAsState(object projectObject, FieldInfo field)
        {
            return InterpretAsState(new ValueSettability(projectObject, field));
        }

        private IFieldState InterpretAsState(ValueSettability settability)
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
