﻿using System.Collections.Generic;
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

		public PluginFactory(ServiceProvider provider)
		{
			this.provider = provider;
		}

		public IEnumerable<IStateFactory> ResolveStateFactories()
		{
			return provider.GetRequiredService<IEnumerable<IStateFactory>>();
		}

		public IEnumerable<IStateViewModelFactory> ResolveStateViewModelFactories()
		{
			return provider.GetRequiredService<IEnumerable<IStateViewModelFactory>>();
		}

		public IEnumerable<IViewFactory> ResolveViewFactories()
		{
			return provider.GetRequiredService<IEnumerable<IViewFactory>>();
		}
	}
}