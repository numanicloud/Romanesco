﻿using Microsoft.Extensions.DependencyInjection;
using Romanesco.BuiltinPlugin.Model.Infrastructure;
using Romanesco.Common.Extensibility.Interfaces;
using Romanesco.Common.Model.Interfaces;

namespace Romanesco.BuiltinPlugin
{
	public class PluginService : IPluginService
	{
		public void ConfigureServices(IServiceCollection services, IApiFactory hostFactory)
		{
			var master = new MasterListContext(hostFactory);
			var factory = new PluginFactory(hostFactory,
				master,
				hostFactory.ResolveCommandHistory(),
				hostFactory.ResolveDataAssemblyRepository());
			
			factory.ConfigureServices(services);
		}
	}
}
