using System;
using System.Collections.Generic;
using System.Text;

namespace Benday.SqlUtils.Api
{
    public interface ITelemetryService
    {
        void TrackEvent(string name, params string[] args);
        void TrackException(Exception ex, params string[] args);
        void Flush();
    }
}
