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
        public IFieldState InterpretAsState(ValueSettability settability, StateInterpretFunc interpret)
        {
            if (settability.Type.IsEnum)
            {
                return new EnumState(settability);
            }
            return null;
        }
    }
}
