using System;
using System.Collections.Generic;
using System.Text;
using Romanesco.Common.Model.Interfaces;

namespace Romanesco.Model.Infrastructure
{
	public static class FactoryProvider
	{
		private static IOpenModelFactory? _openModelFactoryCache;

		public static IOpenModelFactory GetOpenModelFactory(IModelRequirementFactory requirement, IPluginFactory plugin)
		{
			return _openModelFactoryCache ??= new ModelFactory(requirement, plugin);
		}
	}
}
