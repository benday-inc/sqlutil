using Benday.SqlUtils.Api;
using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Text;

namespace Benday.SqlUtils.WpfUi.ViewModel
{
    public class AppInsightsTelemetryService : ITelemetryService
    {
        private TelemetryClient _Client;

        public AppInsightsTelemetryService(TelemetryClient client)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client", "Argument cannot be null.");
            }

            _Client = client;
        }



        public void TrackEvent(string name, params string[] args)
        {
            var properties = ArgumentArrayUtility.ArgsToDictionary(args);

            _Client.TrackEvent(name, properties);
        }

        public void TrackException(Exception ex, params string[] args)
        {
            var properties = ArgumentArrayUtility.ArgsToDictionary(args);

            _Client.TrackException(ex, properties);
        }

        public void Flush()
        {
            _Client.Flush();
        }
    }
}
