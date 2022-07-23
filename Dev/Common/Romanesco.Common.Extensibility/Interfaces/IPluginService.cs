using Microsoft.Extensions.DependencyInjection;
using Romanesco.Common.Model.Interfaces;

namespace Romanesco.Common.Extensibility.Interfaces
{
	public interface IPluginService
	{
		void ConfigureServices(IServiceCollection services, IApiFactory hostFactory);
	}
}
