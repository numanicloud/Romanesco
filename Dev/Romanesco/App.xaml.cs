using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;

namespace Romanesco
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MementoSola.Altseed.Debug.RootExceptionHandler logger;

        protected override void OnStartup(StartupEventArgs e)
        {
            logger = new MementoSola.Altseed.Debug.RootExceptionHandler();

            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            Dispatcher.UnhandledException += Dispatcher_UnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            Common.Model.Helper.OnUnhandledExceptionRaisedInSubscribe += Helper_OnUnhandledExceptionRaisedInSubscribe;

            // .NET Core のバグで、識別子が意図せず日本語になってしまう問題対策(もう直ってるかも)
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            base.OnStartup(e);
        }

        private void Dispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            logger.ProcessError(e.Exception);
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            logger.ProcessError(e.Exception);
        }

        private void Helper_OnUnhandledExceptionRaisedInSubscribe(Exception exception)
        {
            logger.ProcessError(exception);
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            logger.ProcessError(e.Exception);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            logger.ProcessError((Exception)e.ExceptionObject);
        }
    }
}
