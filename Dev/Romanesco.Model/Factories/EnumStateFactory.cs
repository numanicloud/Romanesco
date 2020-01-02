using Romanesco.Common;
using Romanesco.Common.Utility;
using Romanesco.Model.States;
using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Model.Factories
{
    public class EnumStateFactory : Common.IStateFactory
    {
        private readonly CommandHistory history;

        public EnumStateFactory(CommandHistory history)
        {
            this.history = history;
        }

        public IFieldState InterpretAsState(ValueStorage settability, StateInterpretFunc interpret)
        {
            if (settability.Type.IsEnum)
            {
                return new EnumState(settability, history);
            }
            return null;
        }
    }
}
