using Romanesco.Common;
using Romanesco.Common.Utility;
using Romanesco.Model.States;
using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Model.Factories
{
    public class ListStateFactory : IStateFactory
    {
        public IFieldState InterpretAsState(ValueSettability settability, StateInterpretFunc interpret)
        {
            if (settability.Type == typeof(List<>))
            {
                return new ListState(settability, interpret);
            }
            return null;
        }
    }
}
