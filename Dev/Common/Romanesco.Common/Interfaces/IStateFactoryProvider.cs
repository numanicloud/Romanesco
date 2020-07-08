using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Common.Interfaces
{
    public interface IStateFactoryProvider
    {
        IEnumerable<IStateFactory> GetStateFactories();
    }
}
