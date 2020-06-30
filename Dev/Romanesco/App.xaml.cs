using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Extensibility;
using Romanesco.Model;
using Romanesco.Model.Services;
using Romanesco.Startup;
using Romanesco.View.Entry;
using Romanesco.ViewModel;
using System;
using System.Windows;

namespace Romanesco
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private UnhandledExceptionHandler? handler;
		private IHost? host;

		protected override void OnStartup(StartupEventArgs e)
		{
			handler = new UnhandledExceptionHandler(this);

			host = new HostBuilder()
				.ConfigureServices((context, services) =>
				{
					services.AddSingleton<IHostServiceLocator>(provider => new HostServiceLocator(provider))
						.AddSingleton<IServiceLocator>(provider => new HostServiceLocator(provider))
						.AddTransient<PluginLoader>()
						.AddSingleton<MainWindow>();

					new ModelServiceStartUp().ConfigureServices(services);
					new ViewModelServiceStartUp().ConfigureServices(services);
					new ViewServiceStartUp().ConfigureServices(services);

					services.AddHostedService<StartUp>();
				}).Build();

			base.OnStartup(e);
		}

		private async void Application_Startup(object sender, StartupEventArgs e)
		{
			if (host is IHost)
			{
				await host.StartAsync();
			}
			else
			{
				throw new Exception();
			}
		}

		private async void Application_Exit(object sender, ExitEventArgs e)
		{
			if (host is IHost)
			{
				await host.StopAsync(TimeSpan.FromSeconds(1));
			}
		}
	}
}
