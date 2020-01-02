using Romanesco.Common;
using Romanesco.Common.Utility;
using Romanesco.Model.Infrastructure;
using Romanesco.Model.States;
using System;
using System.Collections.Generic;
using System.Text;
using Romanesco.Annotations;
using System.Linq;

namespace Romanesco.Model.Factories
{
    public class ListStateFactory : IStateFactory
    {
        private readonly MasterListContext context;
        private readonly CommandHistory history;

        public ListStateFactory(MasterListContext context, CommandHistory history)
        {
            this.context = context;
            this.history = history;
        }

        public IFieldState InterpretAsState(ValueStorage settability, StateInterpretFunc interpret)
        {
            if (settability.Type.IsGenericType
                && settability.Type.GetGenericTypeDefinition() == typeof(List<>))
            {
                var list = new ListState(settability, interpret, history);
                var attr = settability.Attributes.OfType<EditorMasterAttribute>().FirstOrDefault();
                if (attr != null)
                {
                    context.AddList(new MasterList(attr.MasterName, list, attr.IdMemberName));
                }
                return list;
            }
            return null;
        }
    }
}
