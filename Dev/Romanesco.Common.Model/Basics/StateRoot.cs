using Romanesco.Common.Model.Interfaces;

namespace Romanesco.Common.Model.Basics
{
    public class StateRoot
    {
        public object RootInstance { get; }
        public IFieldState[] States { get; }

        public StateRoot(object rootInstance, IFieldState[] states)
        {
            RootInstance = rootInstance;
            States = states;
        }
    }
}
