using Microsoft.Extensions.DependencyInjection;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.View.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;
using System;

namespace Romanesco.Common.Extensibility
{
	public static class Extensions
	{
		public static IServiceCollection AddStateFactoriesAsSingleton(this IServiceCollection services,
			params Type[] factoryTypes)
		{
			return AddSingletonTypes(services, typeof(IStateFactory), factoryTypes);
		}

		public static IServiceCollection AddViewModelFactoriesAsSingleton(this IServiceCollection services,
			params Type[] factoryTypes)
		{
			return AddSingletonTypes(services, typeof(IStateViewModelFactory), factoryTypes);
		}

		public static IServiceCollection AddViewFactoriesAsSingleton(this IServiceCollection services,
			params Type[] factoryTypes)
		{
			return AddSingletonTypes(services, typeof(IViewFactory), factoryTypes);
		}

		private static IServiceCollection AddSingletonTypes(IServiceCollection services, Type serviceType, Type[] factoryTypes)
		{
			foreach (var item in factoryTypes)
			{
				services.AddSingleton(serviceType, item);
			}
			return services;
		}

		public static IServiceCollection AddSingletons<TService>(this IServiceCollection services,
			params Type[] types)
		{
			foreach (var item in types)
			{
				services.AddSingleton(typeof(TService), item);
			}
			return services;
		}
	}
}
