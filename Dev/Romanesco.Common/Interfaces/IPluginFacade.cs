using Romanesco.Common.Interfaces;
using Romanesco.Common.Utility;
using System.Collections.Generic;

namespace Romanesco.Common
{
    public interface IPluginFacade : IStateFactoryProvider, IStateViewModelFactoryProvider, IViewFactoryProvider
    {
        void LoadContext(ProjectContext context);
    }
}
