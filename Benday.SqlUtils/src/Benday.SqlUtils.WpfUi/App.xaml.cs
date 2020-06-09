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
            LocatorInstance.TelemetryClient.TrackEvent("Shutdown");
            LocatorInstance.TelemetryClient.Flush();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var client = LocatorInstance.TelemetryClient;
            
            if (String.IsNullOrWhiteSpace(SqlUtilSettings.Default.AppInsightsUserId) == true)
            {
                SqlUtilSettings.Default.AppInsightsUserId = Guid.NewGuid().ToString();
                SqlUtilSettings.Default.Save();
            }

            client.Context.User.Id = SqlUtilSettings.Default.AppInsightsUserId;

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            client.Context.GlobalProperties.Add("OSVersion", Environment.OSVersion.ToString());
            client.Context.GlobalProperties.Add("AppName", System.AppDomain.CurrentDomain.FriendlyName);
            client.Context.GlobalProperties.Add("AppVersion", assembly.FullName);
            client.Context.GlobalProperties.Add("CurrentCulture", CultureInfo.CurrentCulture.ToString());

            client.TrackEvent("Startup");
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var temp = new ExceptionTelemetry(e.Exception);
            LocatorInstance.TelemetryClient.TrackException(temp);
        }
    }
}
