using Romanesco.Extensibility;
using Romanesco.View;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;
using Romanesco.Model.Services;
using Romanesco.Common.Model.Infrastructures;
using Microsoft.Extensions.DependencyInjection;

namespace Romanesco
{
	class StartUp : IHostedService
    {
		private readonly PluginLoader pluginLoader;
		private readonly IHostServiceLocator serviceLocator;
		private MainWindow? mainWindow;

		public StartUp(PluginLoader pluginLoader, IHostServiceLocator serviceLocator)
		{
			this.pluginLoader = pluginLoader;
			this.serviceLocator = serviceLocator;
		}

        private IEditorViewContext LoadMainDataContext()
        {
            var extensions = pluginLoader.Load("Plugins");
			var pluginServices = new ServiceCollection();
			pluginServices.AddPlugin(extensions);

			var pluginSL = pluginServices.BuildServiceProvider().GetService<ServiceLocator>();
			serviceLocator.PluginServiceLocator = pluginSL;

			return serviceLocator.GetService<IEditorViewContext>();
        }

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			mainWindow = serviceLocator.GetService<MainWindow>();
			mainWindow.DataContext = LoadMainDataContext();
			mainWindow.Show();
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
			mainWindow?.Close();
		}
	}
}
