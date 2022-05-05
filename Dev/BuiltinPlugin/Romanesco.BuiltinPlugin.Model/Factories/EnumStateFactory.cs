using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;

namespace Romanesco.BuiltinPlugin.Model.Factories
{
    public class EnumStateFactory : IStateFactory
    {
        private readonly CommandHistory history;

        public EnumStateFactory(CommandHistory history)
        {
            this.history = history;
        }

        public IFieldState? InterpretAsState(ValueStorage settability, StateInterpretFunc interpret)
        {
			if (!settability.Type.IsEnum)
			{
				return null;
			}

			return new EnumState(settability, history);
		}
    }
}
