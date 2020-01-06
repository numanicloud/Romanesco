using Romanesco.Annotations;
using Romanesco.BuiltinPlugin.Model.Infrastructure;
using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Romanesco.BuiltinPlugin.Model.Factories
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
