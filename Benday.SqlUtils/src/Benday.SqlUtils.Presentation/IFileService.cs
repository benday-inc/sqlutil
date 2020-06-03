using System;
using System.Collections.Generic;
using System.Text;

namespace Benday.SqlUtils.Presentation
{
    public interface IFileService
    {
        string Filename { get; set; }
        bool ShowSaveFileDialog();
        void SaveFile(string filename, string contents);
    }
}
