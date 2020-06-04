using Benday.SqlUtils.Presentation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Benday.SqlUtils.UnitTests
{
    public class MockFileService : IFileService
    {
        public string Filename
        {
            get; set;
        }

        public bool ShowFileDialogReturnValue { get; set; }
        public bool WasShowFileDialogCalled { get; set; }

        public bool ShowSaveFileDialog()
        {
            WasShowFileDialogCalled = true;
            return ShowFileDialogReturnValue;
        }

        public bool WasSaveFileCalled { get; set; }
        public string SaveFileParameterFilename { get; set; }
        public string SaveFileParameterContents { get; set; }
        public void SaveFile(string filename, string contents)
        {
            WasSaveFileCalled = true;
            SaveFileParameterContents = contents;
            SaveFileParameterFilename = filename;
        }
    }
}
