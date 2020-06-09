using Benday.SqlUtils.Api;
using System;

namespace Benday.SqlUtils.UnitTests.ViewModels
{
    public class MockTelemetryService : ITelemetryService
    {

        public MockTelemetryService()
        {

        }

        public void Flush()
        {
        }

        public void TrackEvent(string name, params string[] args)
        {

        }

        public void TrackException(Exception ex, params string[] args)
        {

        }
    }
}
