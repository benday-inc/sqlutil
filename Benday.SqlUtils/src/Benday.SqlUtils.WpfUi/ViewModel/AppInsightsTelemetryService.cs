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
        private bool _IsEnabled;

        public AppInsightsTelemetryService(TelemetryClient client)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client", "Argument cannot be null.");
            }

            _Client = client;
            _IsEnabled = client.IsEnabled();
        }

        public void TrackEvent(string name, params string[] args)
        {
            if (_IsEnabled == false) return;

            var properties = ArgumentArrayUtility.ArgsToDictionary(args);

            _Client.TrackEvent(name, properties);
        }

        public void TrackException(Exception ex, params string[] args)
        {
            if (_IsEnabled == false) return;

            var properties = ArgumentArrayUtility.ArgsToDictionary(args);

            _Client.TrackException(ex, properties);
        }

        public void Flush()
        {
            _Client.Flush();
        }

        public void SetTelemetry(bool enabled)
        {
            _IsEnabled = enabled;
        }
    }
}
