using Benday.SqlUtils.Api;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Benday.SqlUtils.WpfUi
{
    public class FileService : IFileService
    {
        public string Filename
        {
            get;
            set;
        }

        public void SaveFile(string filename, string contents)
        {
            File.WriteAllText(filename, contents);
        }

        public bool ShowSaveFileDialog()
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
