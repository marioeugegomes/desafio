using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog.Context;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.IO;

namespace Poc.Cliente.Infra.Services
{
    public class LogService : ILogService
    {
        public ILogger Logger { get; }
        private readonly Serilog.IDiagnosticContext _diagnosticContext;
        private const string _messageTemplate = "Integração Logger - {Message}";

        public LogService(
            ILogger<LogService> logger,
            Serilog.IDiagnosticContext diagnosticContext)
        {
            Logger = logger;
            _diagnosticContext = diagnosticContext;
        }

        public async Task InvokeWithCustomPropertiesAsync(Func<Task> function, Dictionary<string, object> customProperties)
        {
            var pushedProperties = PushProperties(customProperties);
            try
            {
                await Task.Run(function);
            }
            finally
            {
                DisposeProperties(pushedProperties);
            }
        }

        public void InvokeWithCustomProperties(Action action, Dictionary<string, object> customProperties)
        {
            var pushedProperties = PushProperties(customProperties);
            try
            {
                action.Invoke();
            }
            finally
            {
                DisposeProperties(pushedProperties);
            }
        }

        public void EnrichDiagnosticContext(string propertyName, string value)
        {
            _diagnosticContext.Set(propertyName, value);
        }

        public HttpResponsePayload LogResponse(
            Func<HttpResponseMessage> requestFunc,
            string requestBody = "",
            bool readBody = true)
        {
            var responseBody = "";
            var dateTimeStarted = DateTime.UtcNow;

            var httpResponse = requestFunc.Invoke();

            var dateTimeFinished = DateTime.UtcNow;

            if (readBody)
            {
                using (var reader = new StreamReader(httpResponse.Content.ReadAsStream()))
                {
                    responseBody = reader.ReadToEnd();
                }
            }

            LogRequestProperties(
                httpResponse,
                requestBody,
                responseBody,
                dateTimeStarted,
                dateTimeFinished
            );

            return new HttpResponsePayload()
            {
                HttpResponse = httpResponse,
                HttpResponseContentString = responseBody
            };
        }

        public async Task<HttpResponsePayload> LogResponseAsync(
            Task<HttpResponseMessage> requestTask,
            string requestBody = "",
            bool readBody = true)
        {
            var responseBody = "";
            var dateTimeStarted = DateTime.UtcNow;

            var httpResponse = await requestTask;

            var dateTimeFinished = DateTime.UtcNow;
            if (readBody)
            {
                responseBody = await httpResponse.Content.ReadAsStringAsync();
            }

            LogRequestProperties(
                httpResponse,
                requestBody,
                responseBody,
                dateTimeStarted,
                dateTimeFinished
            );

            return new HttpResponsePayload()
            {
                HttpResponse = httpResponse,
                HttpResponseContentString = responseBody
            };
        }

        private void LogRequestProperties(
            HttpResponseMessage httpResponse,
            string requestBody,
            string responseBody,
            DateTime dateTimeStarted,
            DateTime dateTimeFinished)
        {
            var requestUri = httpResponse.RequestMessage.RequestUri?.ToString();

            InvokeWithCustomProperties(
                action: () =>
                {
                    Logger.LogInformation(_messageTemplate, $"Handled {requestUri}");
                },
                customProperties: new Dictionary<string, object>()
                {
                    { "RequestUri", requestUri },
                    { "RequestMethod", httpResponse.RequestMessage.Method.ToString() },
                    { "RequestBody", requestBody },
                    { "ResponseBody", responseBody },
                    { "StatusCode", (int)httpResponse.StatusCode },
                    { "DateTimeRequestStarted", dateTimeStarted.ToString("o") },
                    { "DateTimeRequestFinished", dateTimeFinished.ToString("o") }
                }
            );
        }

        private IDisposable[] PushProperties(Dictionary<string, object> customProperties)
        {
            return customProperties
                .Select(p => LogContext.PushProperty(p.Key, p.Value))
                .ToArray();
        }

        private void DisposeProperties(IDisposable[] pushedProperties)
        {
            foreach (var pushedProperty in pushedProperties)
            {
                pushedProperty.Dispose();
            }
        }
    }

}
