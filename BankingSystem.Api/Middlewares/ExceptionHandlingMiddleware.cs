using BankingSystem.Core.Shared;
using BankingSystem.Core.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
using System;

namespace BankingSystem.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ISeqLogger _seqLogger;
        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ISeqLogger seqLogger)
        {
            _next = next;
            _seqLogger = seqLogger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DomainException dex)
            {
                var errorLog = dex.LogMessage;
                _seqLogger.LogError(errorLog.Message, errorLog.Params);

                var problemDetails = new
                {
                    Status = dex.StatusCode,
                    Title = dex.Message,
                    IsSuccess = false
                };

                context.Response.StatusCode = dex.StatusCode;
                await context.Response.WriteAsJsonAsync(problemDetails);
            }
            catch (Exception ex)
            {
                _seqLogger.LogFatal("{Message}{StackTrace}", ex.Message, ex.StackTrace);

                var problemDetails = new
                {
                    Status = 500,
                    Title = "Server Error",
                    IsSuccess = false
                };

                context.Response.StatusCode = problemDetails.Status;
                await context.Response.WriteAsJsonAsync(problemDetails);

            }
        }
    }
}
