using System;
using System.Threading.Tasks;
using System.Windows;
using Romanesco.Common.Model.Helpers;

namespace Romanesco.Startup
{
	public class UnhandledExceptionHandler
	{
        private readonly RootExceptionHandler logger = new RootExceptionHandler();

		public UnhandledExceptionHandler(Application app)
        {
            app.DispatcherUnhandledException += App_DispatcherUnhandledException;
            app.Dispatcher.UnhandledException += Dispatcher_UnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            Helper.OnUnhandledExceptionRaisedInSubscribe += Helper_OnUnhandledExceptionRaisedInSubscribe;
        }

        private void Dispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            logger?.ProcessError(e.Exception);
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            logger?.ProcessError(e.Exception);
        }

        private void Helper_OnUnhandledExceptionRaisedInSubscribe(Exception exception)
        {
            logger?.ProcessError(exception);
        }

        private void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            if (e.Exception != null)
            {
                logger?.ProcessError(e.Exception);
            }
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            logger?.ProcessError((Exception)e.ExceptionObject);
        }
    }
}
