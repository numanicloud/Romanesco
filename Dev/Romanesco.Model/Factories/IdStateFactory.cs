using Romanesco.Annotations;
using Romanesco.Common;
using Romanesco.Common.Utility;
using Romanesco.Model.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romanesco.Model.Factories
{
    public class IdStateFactory : IStateFactory
    {
        private readonly MasterListContext context;

        public IdStateFactory(MasterListContext context)
        {
            this.context = context;
        }

        public IFieldState InterpretAsState(ValueStorage settability, StateInterpretFunc interpret)
        {
            var type = settability.Type;
            var attr = settability.Attributes.OfType<EditorChoiceOfMasterAttribute>().FirstOrDefault();
            if (attr != null)
            {
                if (type == typeof(int))
                {
                    return new States.IntIdChoiceState(settability, attr.MasterName, context);
                }
            }
            return null;
        }
    }
}
