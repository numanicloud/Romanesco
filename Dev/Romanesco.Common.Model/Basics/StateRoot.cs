using Romanesco.Common.Model.Interfaces;

namespace Romanesco.Common.Model.Basics
{
    public class StateRoot
    {
        public object RootInstance { get; set; }
        public IFieldState[] States { get; set; }
    }
}
