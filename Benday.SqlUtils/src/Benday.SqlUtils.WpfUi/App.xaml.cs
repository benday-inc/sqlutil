using Benday.SqlUtils.WpfUi.ViewModel;
using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Benday.SqlUtils.WpfUi
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ViewModelLocator _LocatorInstance;
        public ViewModelLocator LocatorInstance
        {
            get
            {
                if (_LocatorInstance == null)
                {
                    _LocatorInstance = FindResource("Locator") as ViewModelLocator;
                }

                return _LocatorInstance;
            }
        }


        private void Application_Exit(object sender, ExitEventArgs e)
        {
            var duration = DateTime.UtcNow - _AppStartTime;

            LocatorInstance.Telemetry.TrackEvent("Shutdown", "SessionDurationInSeconds", duration.TotalSeconds.ToString());
            LocatorInstance.Telemetry.Flush();
        }

        private DateTime _AppStartTime = DateTime.UtcNow;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            LocatorInstance.Telemetry.TrackEvent("Startup");
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            LocatorInstance.Telemetry.TrackException(e.Exception, nameof(Application_DispatcherUnhandledException));
        }
    }
}
