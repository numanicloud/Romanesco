using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Common {
    public interface IPluginFacade {
        IEnumerable<IStateFactory> GetStateFactories();
        IEnumerable<IStateViewModelFactory> GetStateViewModelFactories();
        IEnumerable<IViewFactory> GetViewFactories();
    }
}
