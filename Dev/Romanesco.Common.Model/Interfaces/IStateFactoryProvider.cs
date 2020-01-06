using System.Collections.Generic;

namespace Romanesco.Common.Model.Interfaces
{
    public interface IStateFactoryProvider
    {
        IEnumerable<IStateFactory> GetStateFactories();
    }
}
