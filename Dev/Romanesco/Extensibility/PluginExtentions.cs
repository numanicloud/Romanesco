using Romanesco.Common.Extensibility.Interfaces;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.View.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;
using Romanesco.Model.ProjectComponents;
using System.Collections.Generic;
using System.Linq;

namespace Romanesco.Extensibility
{
    class PluginExtentions : IStateFactoryProvider, IStateViewModelFactoryProvider, IViewFactoryProvider
    {
        private readonly IPluginFacade[] plugins;

        public PluginExtentions(IPluginFacade[] plugins)
        {
            this.plugins = plugins;
        }

        public IEnumerable<IStateFactory> GetStateFactories(ProjectContextCrawler context)
        {
            return plugins.SelectMany(p => p.GetStateFactories(context));
        }

        public IEnumerable<IStateViewModelFactory> GetStateViewModelFactories()
        {
            return plugins.SelectMany(p => p.GetStateViewModelFactories());
        }

        public IEnumerable<IViewFactory> GetViewFactories()
        {
            return plugins.SelectMany(p => p.GetViewFactories());
        }
    }
}
