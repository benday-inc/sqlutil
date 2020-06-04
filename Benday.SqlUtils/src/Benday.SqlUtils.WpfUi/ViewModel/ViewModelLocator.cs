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

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}