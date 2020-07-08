using Microsoft.Extensions.DependencyInjection;

namespace Romanesco.Common.Extensibility.Interfaces
{
	public interface IPluginService
	{
		void ConfigureServices(IServiceCollection services);
	}
}
