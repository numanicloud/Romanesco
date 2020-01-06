using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Model.States;

namespace Romanesco.Model.Factories
{
    public class EnumStateFactory : IStateFactory
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
