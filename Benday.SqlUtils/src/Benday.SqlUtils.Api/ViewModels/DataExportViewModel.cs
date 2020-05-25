using Benday.Presentation;
using Benday.SqlUtils.Core;
using Benday.SqlUtils.Core.ViewModels;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Benday.SqlUtils.Api.ViewModels
{
    public class DataExportViewModel : DatabaseUtilityViewModelBase
    {
        public DataExportViewModel(IDatabaseConnectionStringRepository repository) :
                    base(repository)
        {

        }

        protected override void OnInitialize()
        {
            _Query = new ViewModelField<string>();
            _GeneratedQuery = new ViewModelField<string>();
            _GenerateIdentityInsert = new ViewModelField<bool>();
            _ExportTableName = new ViewModelField<string>();

            _GeneratedQuery.IsEnabled = false;
            _ExportTableName.IsEnabled = false;
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
                    _RunQueryCommand = new RelayCommand(RunQuery);
                }

                return _RunQueryCommand;
            }
        }
        private void RunQuery()
        {
            throw new NotImplementedException();
        }

        private ICommand _CreateInsertScriptCommand;
        public ICommand CreateInsertScriptCommand
        {
            get
            {
                if (_CreateInsertScriptCommand == null)
                {
                    _CreateInsertScriptCommand = new RelayCommand(CreateInsertScript);
                }

                return _CreateInsertScriptCommand;
            }
        }
        private void CreateInsertScript()
        {
            throw new NotImplementedException();
        }

        private ICommand _CreateMergeIntoScriptCommand;
        public ICommand CreateMergeIntoScriptCommand
        {
            get
            {
                if (_CreateMergeIntoScriptCommand == null)
                {
                    _CreateMergeIntoScriptCommand = new RelayCommand(CreateMergeIntoScript);
                }

                return _CreateMergeIntoScriptCommand;
            }
        }
        private void CreateMergeIntoScript()
        {
            throw new NotImplementedException();
        }
    }
}
