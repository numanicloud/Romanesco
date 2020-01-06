using System.Collections.Generic;

namespace Romanesco.Common.View.Interfaces
{
    public interface IViewFactoryProvider
    {
        IEnumerable<IViewFactory> GetViewFactories();
    }
}
