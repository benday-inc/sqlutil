/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Benday.SqlUtils.WpfUi"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using Benday.SqlUtils.Api;
using Benday.SqlUtils.Presentation;
using Benday.SqlUtils.Presentation.ViewModels;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector;
using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;
using System;
using System.Globalization;
using System.IO;
using System.Windows;

namespace Benday.SqlUtils.WpfUi.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            // ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}
            
            _AppStartTime = DateTime.UtcNow;

            SimpleIoc.Default.Register<DatabaseConnectionsViewModel>();
            SimpleIoc.Default.Register<SearchViewModel>();
            SimpleIoc.Default.Register<DatabaseConnectionStringRepository>();
            SimpleIoc.Default.Register<DataExportViewModel>();
        }

        private DateTime _AppStartTime;

        public double RunningTimeInSeconds
        {
            get
            {
                var duration = DateTime.UtcNow - _AppStartTime;

                return duration.TotalSeconds;
            }
        }

        private DatabaseConnectionsViewModel _ConnectionsEditor;
        public DatabaseConnectionsViewModel ConnectionsEditor
        {
            get
            {
                if (_ConnectionsEditor == null)
                {
                    _ConnectionsEditor = new DatabaseConnectionsViewModel(
                        new DatabaseConnectionStringRepository());
                }
                return _ConnectionsEditor;
            }
        }

        private SearchViewModel _Search;
        public SearchViewModel Search
        {
            get
            {
                if (_Search == null)
                {
                    _Search = new SearchViewModel(
                        new DatabaseConnectionStringRepository(), Telemetry);
                }
                return _Search;
            }
        }

        private DataExportViewModel _DataExport;
        public DataExportViewModel DataExport
        {
            get
            {
                if (_DataExport == null)
                {
                    _DataExport = new DataExportViewModel(
                        new DatabaseConnectionStringRepository(),
                        new SqlServerDatabaseUtility(), 
                        new FileService(), Telemetry);
                }
                return _DataExport;
            }
        }

        private ConfigurationInfo _Configuration;
        public ConfigurationInfo Configuration
        {
            get
            {
                if (_Configuration == null)
                {
                    _Configuration = new ConfigurationInfo();
                }

                return _Configuration;
            }
        }

        private ITelemetryService _Telemetry;
        public ITelemetryService Telemetry
        {
            get
            {
                if (_Telemetry == null)
                {
                    var config = TelemetryConfiguration.CreateDefault();

                    /*
                    var perfCollectorModule = new PerformanceCollectorModule();
                    perfCollectorModule.Counters.Add(new PerformanceCounterCollectionRequest(
                      string.Format(@"\.NET CLR Memory({0})\# GC Handles", System.AppDomain.CurrentDomain.FriendlyName), "GC Handles"));
                    perfCollectorModule.Initialize(config);
                    */

                    var client = new TelemetryClient(config);

                    _Telemetry = new AppInsightsTelemetryService(client);

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
                }

                return _Telemetry;
            }
        }

        private string _ApplicationAppDataPath;
        public string ApplicationAppDataPath
        {
            get
            {
                if (_ApplicationAppDataPath == null)
                {
                    _ApplicationAppDataPath = Path.Combine(
                     Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                     System.AppDomain.CurrentDomain.FriendlyName);

                    if (Directory.Exists(_ApplicationAppDataPath) == false)
                    {
                        Directory.CreateDirectory(_ApplicationAppDataPath);
                    }
                }

                return _ApplicationAppDataPath;
            }
        }
        

        public static void Cleanup()
        {
            
        }
    }
}