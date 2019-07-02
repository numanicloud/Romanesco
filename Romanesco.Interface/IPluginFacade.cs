using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Interface {
    public interface IPluginFacade {
        IEnumerable<IStateFactory> GetStateFactories();
        IEnumerable<IStateViewModelFactory> GetStateViewModelFactories();
        IEnumerable<IViewFactory> GetViewFactories();
    }
}
