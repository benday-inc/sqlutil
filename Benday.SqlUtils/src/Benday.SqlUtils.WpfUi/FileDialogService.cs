using Benday.SqlUtils.Api;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;

namespace Benday.SqlUtils.WpfUi
{
    public class FileDialogService : IFileDialogService
    {
        public string Filename
        {
            get;
            set;
        }

        public bool ShowFileDialog()
        {
            var dialog = new SaveFileDialog();

            var result = dialog.ShowDialog();

            if (result.HasValue == false || result.Value == false)
            {
                return false;
            }
            else
            {
                Filename = dialog.FileName;
                return true;
            }            
        }
    }
}
