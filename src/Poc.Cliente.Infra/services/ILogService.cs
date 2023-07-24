using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Poc.Cliente.Infra.Services
{
    public interface ILogService
    {
        ILogger Logger { get; }
        Task InvokeWithCustomPropertiesAsync(Func<Task> function, Dictionary<string, object> customProperties);
        void InvokeWithCustomProperties(Action action, Dictionary<string, object> customProperties);
        void EnrichDiagnosticContext(string propertyName, string value);
        Task<HttpResponsePayload> LogResponseAsync(
            Task<HttpResponseMessage> requestTask,
            string requestBody = "",
            bool readBody = true);
        HttpResponsePayload LogResponse(
            Func<HttpResponseMessage> requestFunc,
            string requestBody = "",
            bool readBody = true);
    }

    public class HttpResponsePayload
    {
        public HttpResponseMessage HttpResponse { get; set; }
        public string HttpResponseContentString { get; set; }
    }
}
