using Romanesco.Annotations;
using Romanesco.BuiltinPlugin.Model.Infrastructure;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using System.Linq;

namespace Romanesco.BuiltinPlugin.Model.Factories
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
