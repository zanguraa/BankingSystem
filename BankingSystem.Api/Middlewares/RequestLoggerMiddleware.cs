using BankingSystem.Core.Shared;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace BankingSystem.Api.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ISeqLogger _seqLogger;
        private readonly bool _logRequestBody;
        private readonly bool _logResponseBody;

        public RequestResponseLoggingMiddleware(
            RequestDelegate next,
            ISeqLogger seqLogger,
            IConfiguration configuration)
        {
            _next = next;
            _seqLogger = seqLogger;
            _logRequestBody = configuration.GetValue<bool>("Logging:LogRequestBody", false);
            _logResponseBody = configuration.GetValue<bool>("Logging:LogResponseBody", false);
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = Guid.NewGuid().ToString();
            context.Items["CorrelationId"] = correlationId;

            await LogRequest(context, correlationId);

            var originalResponseBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            await LogResponse(context, correlationId);

            await responseBody.CopyToAsync(originalResponseBodyStream);
        }

        private async Task LogRequest(HttpContext context, string correlationId)
        {
            var request = context.Request;

            var logData = new
            {
                CorrelationId = correlationId,
                RequestMethod = request.Method,
                RequestPath = request.Path,
                RequestHeaders = request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()),
                RequestBody = _logRequestBody ? await GetRequestBody(request) : null
            };

            _seqLogger.LogInfo(JsonConvert.SerializeObject(logData));
        }

        private async Task LogResponse(HttpContext context, string correlationId)
        {
            var response = context.Response;
            response.Body.Seek(0, SeekOrigin.Begin);

            var logData = new
            {
                CorrelationId = correlationId,
                StatusCode = response.StatusCode,
                ResponseHeaders = response.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()),
                ResponseBody = _logResponseBody ? await GetResponseBody(response) : null
            };

            _seqLogger.LogInfo(JsonConvert.SerializeObject(logData));

            response.Body.Seek(0, SeekOrigin.Begin);
        }

        private async Task<string> GetRequestBody(HttpRequest request)
        {
            request.EnableBuffering();
            var body = await new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true).ReadToEndAsync();
            request.Body.Position = 0;
            return body;
        }

        private async Task<string> GetResponseBody(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var body = await new StreamReader(response.Body, Encoding.UTF8, leaveOpen: true).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return body;
        }
    }
}