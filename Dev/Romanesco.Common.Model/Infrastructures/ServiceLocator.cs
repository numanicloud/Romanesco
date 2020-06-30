using Microsoft.Extensions.DependencyInjection;
using Romanesco.Common.Model.Interfaces;
using System;

namespace Romanesco.Common.Model.Infrastructures
{
	public class ServiceLocator : IServiceLocator
	{
		private readonly IServiceProvider serviceProvider;

		public ServiceLocator(IServiceCollection services)
		{
			this.serviceProvider = services.BuildServiceProvider();
		}

		public T CreateInstance<T>(params object[] args)
		{
			return ActivatorUtilities.CreateInstance<T>(serviceProvider, args);
		}

		public T GetService<T>() where T : class
		{
			return serviceProvider.GetRequiredService<T>();
		}

		public T GetServiceOptional<T>() where T : class
		{
			return serviceProvider.GetService<T>();
		}
	}
}
