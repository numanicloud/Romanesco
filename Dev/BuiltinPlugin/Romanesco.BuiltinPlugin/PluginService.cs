using System;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
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

			var importer = new GenericHostImporter(services);
			factory.Export(importer);
		}
	}

	public class GenericHostImporter : Imfact.Annotations.IServiceImporter
	{
		private readonly IServiceCollection _services;

		public GenericHostImporter(IServiceCollection services)
		{
			_services = services;
		}

		public void Import<TInterface>(Func<TInterface> resolver)
			where TInterface : class
		{
			_services.AddTransient(_ => resolver());
		}
	}
}
