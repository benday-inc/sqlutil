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
using System.IO;

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
            
            SimpleIoc.Default.Register<DatabaseConnectionsViewModel>();
            SimpleIoc.Default.Register<SearchViewModel>();
            SimpleIoc.Default.Register<DatabaseConnectionStringRepository>();
            SimpleIoc.Default.Register<DataExportViewModel>();
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
                        new DatabaseConnectionStringRepository());
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
                        new FileService());
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

        private TelemetryClient _TelemetryClient;
        public TelemetryClient TelemetryClient
        {
            get
            {
                if (_TelemetryClient == null)
                {
                    var config = TelemetryConfiguration.CreateDefault();

                    var perfCollectorModule = new PerformanceCollectorModule();
                    perfCollectorModule.Counters.Add(new PerformanceCounterCollectionRequest(
                      string.Format(@"\.NET CLR Memory({0})\# GC Handles", System.AppDomain.CurrentDomain.FriendlyName), "GC Handles"));
                    perfCollectorModule.Initialize(config);

                    _TelemetryClient = new TelemetryClient(config);
                }

                return _TelemetryClient;
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