using Benday.Presentation;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Benday.SqlUtils.Api;

namespace Benday.SqlUtils.Presentation.ViewModels
{

    public class DataExportViewModel : DatabaseUtilityViewModelBase
    {
        private IDatabaseUtility _DatabaseUtility;
        private IFileService _FileService;

        private SqlDataExport _Exporter;

        public DataExportViewModel(
            IMessageManager msgManager, 
            IQueryRunner queryRunner,
            IDatabaseConnectionStringRepository repository,
            IDatabaseUtility queryExecuter,
            IFileService fileDialogService,
            ITelemetryService telemetry) :
                    base(msgManager, queryRunner, repository, telemetry)
        {
            if (queryExecuter == null)
            {
                throw new ArgumentNullException("queryExecuter", "Argument cannot be null.");
            }

            if (fileDialogService == null)
            {
                throw new ArgumentNullException("fileDialogService", "Argument cannot be null.");
            }

            _DatabaseUtility = queryExecuter;
            _FileService = fileDialogService;
        }

        protected override void InitializeProperties()
        {
            base.InitializeProperties();

            _Query = new ViewModelField<string>();
            _GeneratedQuery = new ViewModelField<string>();
            _GenerateIdentityInsert = new ViewModelField<bool>();
            _ExportTableName = new ViewModelField<string>();
            _Message = new ViewModelField<string>();
            _SaveToFileName = new ViewModelField<string>();
            _QueryResults = new ViewModelField<DataTable>();

            _GeneratedQuery.IsEnabled = false;
            _ExportTableName.IsEnabled = false;
            _Message.IsVisible = false;
            _QueryResults.IsEnabled = false;
            _SaveToFileName.IsEnabled = false;

            DatabaseConnections.OnItemSelected += DatabaseConnections_OnItemSelected;
        }

        private const string QueryPropertyName = "Query";

        private ViewModelField<string> _Query;
        public ViewModelField<string> Query
        {
            get
            {
                return _Query;
            }
            set
            {
                _Query = value;
                RaisePropertyChanged(QueryPropertyName);
            }
        }

        private const string ExportTableNamePropertyName = "ExportTableName";

        private ViewModelField<string> _ExportTableName;
        public ViewModelField<string> ExportTableName
        {
            get
            {
                return _ExportTableName;
            }
            set
            {
                _ExportTableName = value;
                RaisePropertyChanged(ExportTableNamePropertyName);
            }
        }

        private const string GenerateIdentityInsertPropertyName = "GenerateIdentityInsert";

        private ViewModelField<bool> _GenerateIdentityInsert;
        public ViewModelField<bool> GenerateIdentityInsert
        {
            get
            {
                return _GenerateIdentityInsert;
            }
            set
            {
                _GenerateIdentityInsert = value;
                RaisePropertyChanged(GenerateIdentityInsertPropertyName);
            }
        }

        private const string GeneratedQueryPropertyName = "GeneratedQuery";

        private ViewModelField<string> _GeneratedQuery;
        public ViewModelField<string> GeneratedQuery
        {
            get
            {
                return _GeneratedQuery;
            }
            set
            {
                _GeneratedQuery = value;
                RaisePropertyChanged(GeneratedQueryPropertyName);
            }
        }

        private ICommand _RunQueryCommand;
        public ICommand RunQueryCommand
        {
            get
            {
                if (_RunQueryCommand == null)
                {
                    _RunQueryCommand = new ExceptionHandlingRelayCommand(Messages, RunQuery);
                }

                return _RunQueryCommand;
            }
        }

        private void DatabaseConnections_OnItemSelected(object sender, EventArgs e)
        {
            SelectDatabaseConnectionAndInitialize();
        }

        private void SelectDatabaseConnectionAndInitialize()
        {
            var value = DatabaseConnections.SelectedItem;

            if (value != null)
            {
                _DatabaseUtility.Initialize(value.ConnectionString);
            }
        }

        private void InitializeExporter()
        {
            SelectDatabaseConnectionAndInitialize(); 
            _Exporter = new SqlDataExport(_DatabaseUtility, Query.Value);            
        }

        private void RunQuery()
        {
            if (String.IsNullOrWhiteSpace(Query.Value) == true)
            {
                Query.ValidationMessage = "Query is empty";
                Query.IsValid = false;
            }
            else
            {
                Query.IsValid = true;

                Telemetry.TrackEvent(
                    $"Data Export - Run Query");

                InitializeExporter();

                PopulateExportTableName();

                _QueryResults.Value = _Exporter.QueryResults;
            }
        }

        private const string QueryResultsPropertyName = "QueryResults";

        private ViewModelField<DataTable> _QueryResults;
        public ViewModelField<DataTable> QueryResults
        {
            get
            {
                return _QueryResults;
            }
            set
            {
                _QueryResults = value;
                RaisePropertyChanged(QueryResultsPropertyName);
            }
        }

        private const string TableDescriptionPropertyName = "TableDescription";

        private const string MessagePropertyName = "Message";

        private ViewModelField<string> _Message;
        public ViewModelField<string> Message
        {
            get
            {
                return _Message;
            }
            set
            {
                _Message = value;
                RaisePropertyChanged(MessagePropertyName);
            }
        }

        private ICommand _CreateInsertScriptCommand;
        public ICommand CreateInsertScriptCommand
        {
            get
            {
                if (_CreateInsertScriptCommand == null)
                {
                    _CreateInsertScriptCommand = new ExceptionHandlingRelayCommand(Messages, CreateInsertScript);
                }

                return _CreateInsertScriptCommand;
            }
        }
        private void CreateInsertScript()
        {
            if (ExportTableName.IsValid == true &&
                QueryResults.Value != null)
            {
                var script = GetInsertScript();

                GeneratedQuery.IsEnabled = true;
                GeneratedQuery.Value = script;
            }
            else
            {
                GeneratedQuery.IsEnabled = false;
                GeneratedQuery.Value = String.Empty;
                Message.IsVisible = true;
                Message.Value = "Table name to export is not valid, query data has not been loaded, and/or table description has not been loaded.";
            }
        }

        private ICommand _CreateMergeIntoScriptCommand;
        public ICommand CreateMergeIntoScriptCommand
        {
            get
            {
                if (_CreateMergeIntoScriptCommand == null)
                {
                    _CreateMergeIntoScriptCommand = new ExceptionHandlingRelayCommand(Messages, CreateMergeIntoScript);
                }

                return _CreateMergeIntoScriptCommand;
            }
        }
        private void CreateMergeIntoScript()
        {
            if (ExportTableName.IsValid == true &&
                QueryResults.Value != null)
            {
                var script = GetMergeIntoScript();

                GeneratedQuery.IsEnabled = true;
                GeneratedQuery.Value = script;
            }
            else
            {
                GeneratedQuery.IsEnabled = false;
                GeneratedQuery.Value = String.Empty;
                Message.IsVisible = true;
                Message.Value = "Table name to export is not valid, query data has not been loaded, and/or table description has not been loaded.";
            }
        }

        private void PopulateExportTableName()
        {
            var tableName = _Exporter.ExportTableName;

            if (String.IsNullOrWhiteSpace(tableName) == true ||
                tableName == "(table name not found)")
            {
                this.ExportTableName.Value = "(table name not found)";
                this.ExportTableName.IsValid = false;
                this.ExportTableName.IsEnabled = false;
                this.ExportTableName.ValidationMessage = "Could not locate table name in query.";
            }
            else
            {
                this.ExportTableName.Value = tableName;
                this.ExportTableName.IsValid = true;
                this.ExportTableName.IsEnabled = false;
            }
        }

        private static StringBuilder FormatTableName(string tableName)
        {
            StringBuilder tableNameFormatter = new StringBuilder();

            if (tableName.StartsWith("[") == false)
            {
                tableNameFormatter.Append("[");
            }

            tableNameFormatter.Append(tableName);

            if (tableName.EndsWith("]") == false)
            {
                tableNameFormatter.Append("]");
            }

            return tableNameFormatter;
        }

        private string GetInsertScript()
        {
            Telemetry.TrackEvent(
                $"Data Export - Create Insert Script",
                "GenerateIdentityInsert", GenerateIdentityInsert.Value.ToString());

            return _Exporter.GetInsertScript(GenerateIdentityInsert.Value);
        }

        private string GetMergeIntoScript()
        {
            Telemetry.TrackEvent(
                $"Data Export - Create Merge Into Script");

            return _Exporter.GetMergeIntoScript();
        }

        private ICommand _SaveScriptCommand;
        public ICommand SaveScriptCommand
        {
            get
            {
                if (_SaveScriptCommand == null)
                {
                    _SaveScriptCommand = new ExceptionHandlingRelayCommand(Messages, SaveScript);
                }

                return _SaveScriptCommand;
            }
        }
        private void SaveScript()
        {
            if (this.GeneratedQuery.IsEnabled == true && this.GeneratedQuery.IsValid == true &&
                this.GeneratedQuery.IsVisible == true)
            {
                var result = _FileService.ShowSaveFileDialog();

                if (result == true)
                {
                    SaveToFileName.Value = _FileService.Filename;
                    SaveToFileName.IsVisible = true;

                    _FileService.SaveFile(_FileService.Filename, _GeneratedQuery.Value);
                }
            }
        }

        private const string SaveToFileNamePropertyName = "SaveToFileName";

        private ViewModelField<string> _SaveToFileName;
        public ViewModelField<string> SaveToFileName
        {
            get
            {
                return _SaveToFileName;
            }
            set
            {
                _SaveToFileName = value;
                RaisePropertyChanged(SaveToFileNamePropertyName);
            }
        }

        private ICommand _CopyGeneratedQueryToClipboardCommand;
        public ICommand CopyGeneratedQueryToClipboardCommand
        {
            get
            {
                if (_CopyGeneratedQueryToClipboardCommand == null)
                {
                    _CopyGeneratedQueryToClipboardCommand = new ExceptionHandlingRelayCommand(Messages, CopyGeneratedQueryToClipboard);
                }

                return _CopyGeneratedQueryToClipboardCommand;
            }
        }
        private void CopyGeneratedQueryToClipboard()
        {
            Clipboard.SetText(_GeneratedQuery.Value);
        }
    }
}
