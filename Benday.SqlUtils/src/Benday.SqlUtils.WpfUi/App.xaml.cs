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
            Dictionary<string, string> properties = new Dictionary<string, string>();

            var duration = DateTime.UtcNow - _AppStartTime;

            properties.Add("SessionDurationInSeconds", duration.TotalSeconds.ToString());

            LocatorInstance.TelemetryClient.TrackEvent("Shutdown", properties);
            LocatorInstance.TelemetryClient.Flush();
        }

        private DateTime _AppStartTime = DateTime.UtcNow;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var client = LocatorInstance.TelemetryClient;
            
            if (String.IsNullOrWhiteSpace(SqlUtilSettings.Default.AppInsightsUserId) == true)
            {
                SqlUtilSettings.Default.AppInsightsUserId = Guid.NewGuid().ToString();
                SqlUtilSettings.Default.Save();
            }

            bool isFirstUserSession = false;

            if (String.IsNullOrWhiteSpace(SqlUtilSettings.Default.AppInsightsUserSessionIsFirst) == true)
            {
                isFirstUserSession = true;

                SqlUtilSettings.Default.AppInsightsUserSessionIsFirst = "false";
                SqlUtilSettings.Default.Save();
            }

            client.Context.User.Id = SqlUtilSettings.Default.AppInsightsUserId;

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            client.Context.GlobalProperties.Add("OSVersion", Environment.OSVersion.ToString());
            client.Context.GlobalProperties.Add("AppName", System.AppDomain.CurrentDomain.FriendlyName);
            client.Context.GlobalProperties.Add("AppVersion", assembly.FullName);
            client.Context.GlobalProperties.Add("CurrentCulture", CultureInfo.CurrentCulture.ToString());

            client.Context.GlobalProperties.Add("TimeZone", TimeZoneInfo.Local.ToString());

            client.Context.GlobalProperties.Add("IsFirstTimeUsingApp", isFirstUserSession.ToString());

            client.Context.GlobalProperties.Add("PrimaryScreenWidth", SystemParameters.PrimaryScreenWidth.ToString());
            client.Context.GlobalProperties.Add("PrimaryScreenHeight", SystemParameters.PrimaryScreenHeight.ToString());

            client.Context.Session.Id = Guid.NewGuid().ToString();
            client.Context.Session.IsFirst = isFirstUserSession;

            client.TrackEvent("Startup");
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var temp = new ExceptionTelemetry(e.Exception);
            LocatorInstance.TelemetryClient.TrackException(temp);
        }
    }
}
