
using BankingSystem.Core.Shared.Exceptions;

namespace BankingSystem.Api.Middlewares
{
	public class ExceptionHandlingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionHandlingMiddleware> _logger;

		public ExceptionHandlingMiddleware(
			RequestDelegate next,
			ILogger<ExceptionHandlingMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (NotFoundException notFoundException)
			{
				await ReturnErrorResult(context, notFoundException, StatusCodes.Status404NotFound);
			}
			catch (DomainException dex)
			{
				await ReturnErrorResult(context, dex, StatusCodes.Status500InternalServerError);
			}
			catch (Exception ex)
			{
				await ReturnErrorResult(context, ex, StatusCodes.Status500InternalServerError);
			}
		}

		private async Task ReturnErrorResult(HttpContext context, DomainException exception, int statusCode)
		{
			_logger.LogError(
				exception, "Exception occurred: {Message}", exception.Message);

			var problemDetails = new
			{
				Status = statusCode,
				Title = exception.Message,
				IsSuccess = true
			};

			context.Response.StatusCode = statusCode;

			await context.Response.WriteAsJsonAsync(problemDetails);
		}

		private async Task ReturnErrorResult(HttpContext context, Exception exception, int statusCode)
		{
			_logger.LogError(
				exception, "Exception occurred: {Message}", exception.Message);

			var problemDetails = new
			{
				Status = statusCode,
				Title = "Server Error",
				IsSuccess = true
			};

			context.Response.StatusCode = statusCode;

			await context.Response.WriteAsJsonAsync(problemDetails);
		}
	}
}
