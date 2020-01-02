﻿using System;
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
        private readonly CommandHistory history;

        public PrimitiveStateFactory(CommandHistory history)
        {
            this.history = history;
        }

        public IFieldState InterpretAsState(ValueSettability settability,
            StateInterpretFunc interpret)
        {
            if (settability.Type == typeof(int)) return new IntState(settability, history);
            if (settability.Type == typeof(float)) return new FloatState(settability, history);
            if (settability.Type == typeof(string)) return new StringState(settability, history);
            if (settability.Type == typeof(bool)) return new BoolState(settability, history);

            if (settability.Type == typeof(byte)) return new ByteState(settability, history);
            if (settability.Type == typeof(short)) return new ShortState(settability, history);
            if (settability.Type == typeof(long)) return new LongState(settability, history);
            if (settability.Type == typeof(double)) return new DoubleState(settability, history);
            return null;
        }
    }
}