using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Romanesco.Extensibility;
using Romanesco.Infrastructure;

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
			var viewFactory = ResolveFactory(GetPluginServices());

			mainWindow = host.ResolveMainWindow();
			mainWindow.DataContext = viewFactory.ResolveEditorViewContext();
			mainWindow.Show();
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
			mainWindow?.Close();
		}

		private View.Infrastructure.IOpenViewFactory ResolveFactory(PluginFactory pluginFactory)
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

			return viewFactory;
		}

		public PluginFactory GetPluginServices()
		{
			var pluginLoader = new PluginLoader();
			var extensions = pluginLoader.Load("Plugins");
			var serviceCollection = new ServiceCollection();
			extensions.ConfigureServices(serviceCollection);
			return new PluginFactory(serviceCollection);
		}
	}
}
