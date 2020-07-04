using Microsoft.Extensions.DependencyInjection;
using Romanesco.Extensibility;

namespace Romanesco
{
	static class Helpers
	{
		public static IServiceCollection AddPlugin(this IServiceCollection services, string pluginDirPath)
		{
			var pluginLoader = new PluginLoader();
			var ext = pluginLoader.Load(pluginDirPath);
			ext.ConfigureServices(services);
			return services;
		}
	}
}
