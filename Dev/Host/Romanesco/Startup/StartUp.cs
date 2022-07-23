using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NacHelpers.Extensions;
using Numani.TypedFilePath;
using Numani.TypedFilePath.Interfaces;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Extensibility;
using Romanesco.Infrastructure;
using Romanesco.Model.Infrastructure;
using Romanesco.View.Infrastructure;

namespace Romanesco.Startup
{
	internal class StartUp : IHostedService
	{
		private readonly IHostFactory host;
		private readonly Application _application;
		private MainWindow? mainWindow;

		public StartUp(IHostFactory host, Application application)
		{
			this.host = host;
			_application = application;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			var api = new ApiFactory(host);
			var pluginLoader = new PluginLoader();
			var extensions = pluginLoader.Load(
				("Plugins".AsDirectoryPath() as IRelativeDirectoryPath)!);
			
			var serviceCollection = new ServiceCollection();
			extensions.ConfigureServices(serviceCollection, api);

			var pluginServices = serviceCollection.BuildServiceProvider();
			var pluginFactory = new PluginFactory(pluginServices, api);
			var (model, view) = ResolveFactory(pluginFactory);
			api.ModelFactory = model;
			api.Provider = pluginServices;

			api.OnProjectChangedProperty.Value = model.ResolveProjectSwitcher().BeforeResetProject;

			var viewContext = view.ResolveEditorViewContext();
			mainWindow = host.ResolveMainWindow();
			mainWindow.DataContext = viewContext;
			mainWindow.Show();

			InstallResourceDictionaries(pluginFactory);
		}

		private void InstallResourceDictionaries(PluginFactory pluginFactory)
		{
			var resources = pluginFactory.ResolveResourceDictionaryFactories()
				.SelectMany(x => x.Get());

			var dic = new Dictionary<object, ResourceDictionary>();
			foreach (var resource in resources)
			{
				foreach (var key in resource.Keys)
				{
					if (resource[key] is not ResourceDictionary inner) continue;

					if (dic.TryGetValue(key, out var merged))
					{
						merged.MergedDictionaries.Add(inner);
					}
					else
					{
						dic.Add(key, new ResourceDictionary()
						{
							MergedDictionaries = { inner }
						});
					}
				}
			}

			var resultingResource = new ResourceDictionary();
			foreach (var key in dic.Keys)
			{
				resultingResource.Add(key, dic[key]);
			}

			_application.Resources.MergedDictionaries.Add(resultingResource);
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
			mainWindow?.Close();
		}

		private (IOpenModelFactory, IOpenViewFactory) ResolveFactory(PluginFactory pluginFactory)
		{
			var modelRequirement = new ModelRequirementFactory(host);
			var modelFactory = Model.Infrastructure.FactoryProvider
				.GetOpenModelFactory(modelRequirement, pluginFactory);

			var viewModelRequirement = new ViewModelRequirementFactory(modelFactory);
			var viewModelFactory = ViewModel.Infrastructure.FactoryProvider
				.GetOpenViewModelFactory(viewModelRequirement, pluginFactory);

			var viewRequirement = new ViewRequirementFactory(viewModelFactory);
			var viewFactory = View.Infrastructure.FactoryProvider
				.GetOpenViewFactory(viewRequirement, pluginFactory);

			modelRequirement.ViewModel = viewModelFactory;

			return (modelFactory, viewFactory);
		}
	}
}
