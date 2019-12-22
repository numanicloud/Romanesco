using Romanesco.Common.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Common {
    public interface IPluginFacade {
        void LoadContext(ProjectContext context);
        IEnumerable<IStateFactory> GetStateFactories();
        IEnumerable<IStateViewModelFactory> GetStateViewModelFactories();
        IEnumerable<IViewFactory> GetViewFactories();
    }
}
