using System;
using System.Collections.Generic;
using System.Text;

namespace Benday.SqlUtils.Api
{
    public interface IFileDialogService
    {
        string Filename { get; set; }
        bool ShowFileDialog();
    }
}
