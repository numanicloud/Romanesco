using Microsoft.Extensions.DependencyInjection;
using Romanesco.Common.Model.Interfaces;
using System;

namespace Romanesco.Model.Services
{
	public class HostServiceLocator : IServiceLocator, IHostServiceLocator
	{
		private readonly IServiceProvider serviceProvider;

		public IServiceLocator? PluginServiceLocator { get; set; }

		public HostServiceLocator(IServiceProvider provider)
		{
			serviceProvider = provider;
		}

		public T GetService<T>() where T : class
		{
			return serviceProvider.GetService<T>() is T service ? service
				: PluginServiceLocator?.GetService<T>() is T service2 ? service2
				: throw new InvalidOperationException();
		}

		public T? GetServiceOptional<T>() where T : class
		{
			return serviceProvider.GetService<T>() is T service ? service
				: PluginServiceLocator?.GetServiceOptional<T>();
		}

		public T CreateInstance<T>(params object[] args)
		{
			// TODO: PluginServiceLocatorも見に行くようにしたい
			return ActivatorUtilities.CreateInstance<T>(serviceProvider, args);
		}
	}
}
