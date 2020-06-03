using System;
using System.Collections.Generic;
using System.Text;

namespace Benday.SqlUtils.Api
{
    public interface IFileService
    {
        string Filename { get; set; }
        bool ShowSaveFileDialog();
        void SaveFile(string filename, string contents);
    }
}
