using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Extensibility;
using Romanesco.Infrastructure;
using Romanesco.Model.Infrastructure;
using Romanesco.View.Infrastructure;

namespace Romanesco.Startup
{
	internal class StartUp : IHostedService
	{
		private readonly HostFactory host;
		private MainWindow? mainWindow;

		public StartUp(HostFactory host)
		{
			this.host = host;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			var api = new ApiFactory(host);
			var pluginServices = GetPluginServices(api);
			var (model, view) = ResolveFactory(new PluginFactory(pluginServices));
			api.ModelFactory = model;
			api.Provider = pluginServices;

			mainWindow = host.ResolveMainWindow();
			mainWindow.DataContext = view.ResolveEditorViewContext();
			mainWindow.Show();
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

		public ServiceProvider GetPluginServices(IApiFactory api)
		{
			var pluginLoader = new PluginLoader();
			var extensions = pluginLoader.Load("Plugins");

			var serviceCollection = new ServiceCollection();
			extensions.ConfigureServices(serviceCollection, api);

			return serviceCollection.BuildServiceProvider();
		}
	}
}
