﻿namespace Romanesco.ViewModel.Infrastructure
{
	public static class FactoryProvider
	{
		private static IOpenViewModelFactory? _openViewModelFactory;

		public static IOpenViewModelFactory GetOpenViewModelFactory(IViewModelRequirement requirement,
			IPluginFactory plugin)
		{
			return _openViewModelFactory ??= new ViewModelFactory(plugin, requirement);
		}
	}
}
