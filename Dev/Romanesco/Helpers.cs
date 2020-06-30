using Microsoft.Extensions.DependencyInjection;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Infrastructures;
using Romanesco.Extensibility;

namespace Romanesco
{
	static class Helpers
	{
		public static IServiceCollection AddPlugin(this IServiceCollection services,
			PluginExtentions plugins)
		{
			var pluginServices = new ServiceCollection();
			plugins.ConfigureServices(pluginServices);

			services.AddSingleton<CommandHistory>();
			services.AddSingleton(provider => new ServiceLocator(services));

			return services;
		}
	}
}
