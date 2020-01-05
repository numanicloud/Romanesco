using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Common.Interfaces
{
    public interface IViewFactoryProvider
    {
        IEnumerable<IViewFactory> GetViewFactories();
    }
}
