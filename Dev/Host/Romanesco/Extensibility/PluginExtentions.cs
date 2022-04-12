using Microsoft.Extensions.DependencyInjection;
using Romanesco.Common.Extensibility.Interfaces;
using Romanesco.Common.Model.Interfaces;

namespace Romanesco.Extensibility
{
	internal class PluginExtentions
	{
		private readonly IPluginService[] pluginsServices;

		public PluginExtentions(IPluginService[] plugins)
		{
			pluginsServices = plugins;
		}

		public void ConfigureServices(IServiceCollection serviceCollection, IApiFactory hostFactory)
		{
			foreach (var plugin in pluginsServices)
			{
				plugin.ConfigureServices(serviceCollection, hostFactory);
			}
		}
	}
}
