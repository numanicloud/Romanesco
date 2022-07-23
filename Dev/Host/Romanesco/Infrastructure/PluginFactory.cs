using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.View.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;

namespace Romanesco.Infrastructure
{
	class PluginFactory : Model.Infrastructure.IPluginFactory,
		ViewModel.Infrastructure.IPluginFactory,
		View.Infrastructure.IPluginFactory
	{
		private readonly ServiceProvider provider;
		private readonly ApiFactory _api;

		public PluginFactory(ServiceProvider provider, ApiFactory api)
		{
			this.provider = provider;
			_api = api;
		}

		public IEnumerable<IStateFactory> ResolveStateFactories()
		{
			return provider.GetRequiredService<IEnumerable<IStateFactory>>();
		}

		public ILoadingStateProvider ResolveLoadingStateProvider()
		{
			return _api;
		}

		public IEnumerable<IStateViewModelFactory> ResolveStateViewModelFactories()
		{
			return provider.GetRequiredService<IEnumerable<IStateViewModelFactory>>();
		}

		public IEnumerable<IViewFactory> ResolveViewFactories()
		{
			return provider.GetRequiredService<IEnumerable<IViewFactory>>();
		}

		public IEnumerable<IResourceDictionaryFactory> ResolveResourceDictionaryFactories()
		{
			return provider.GetRequiredService<IEnumerable<IResourceDictionaryFactory>>();
		}

		public IEnumerable<IRootViewFactory> ResolveRootViewFactories()
		{
			return provider.GetRequiredService<IEnumerable<IRootViewFactory>>();
		}
	}
}
