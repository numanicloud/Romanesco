using Romanesco.View;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;
using Romanesco.Model.Services;

namespace Romanesco
{
	class StartUp : IHostedService
    {
		private readonly IHostServiceLocator serviceLocator;
		private MainWindow? mainWindow;

		public StartUp(IHostServiceLocator serviceLocator)
		{
			this.serviceLocator = serviceLocator;
		}

        private IEditorViewContext LoadMainDataContext()
        {
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
