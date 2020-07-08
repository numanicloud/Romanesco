using Microsoft.Extensions.DependencyInjection;
using Romanesco.Common.Extensibility.Interfaces;

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
