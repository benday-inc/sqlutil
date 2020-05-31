using Benday.SqlUtils.Api;
using System;
using System.Collections.Generic;
using System.Text;

namespace Benday.SqlUtils.UnitTests
{
    public class MockFileDialogService : IFileDialogService
    {
        public string Filename
        {
            get; set;
        }

        public bool ShowFileDialogReturnValue { get; set; }
        public bool WasShowFileDialogCalled { get; set; }

        public bool ShowFileDialog()
        {
            WasShowFileDialogCalled = true;
            return ShowFileDialogReturnValue;
        }
    }
}
