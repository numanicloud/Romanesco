using Romanesco.Common.Model.Interfaces;

namespace Romanesco.Model.Services
{
	public interface IHostServiceLocator : IServiceLocator
	{
		IServiceLocator? PluginServiceLocator { get; set; }
	}
}
