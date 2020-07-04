using Microsoft.Extensions.DependencyInjection;
using Romanesco.Common.Extensibility.Interfaces;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.View.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;
using Romanesco.Model.ProjectComponents;
using System.Collections.Generic;
using System.Linq;

namespace Romanesco.Extensibility
{
    internal class PluginExtentions
    {
		private readonly IPluginService[] pluginsServices;

		public PluginExtentions(IPluginService[] plugins)
		{
			pluginsServices = plugins;
		}

        public void ConfigureServices(IServiceCollection serviceCollection)
		{
			foreach (var plugin in pluginsServices)
			{
                plugin.ConfigureServices(serviceCollection);
			}
		}
    }
}
