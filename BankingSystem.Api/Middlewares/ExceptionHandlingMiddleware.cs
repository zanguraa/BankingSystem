using BankingSystem.Core.Shared;
using BankingSystem.Core.Shared.Exceptions;

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
            catch (NotFoundException notFoundException)
            {
                await ReturnErrorResult(context, notFoundException, StatusCodes.Status404NotFound);
            }
            catch (BankAccountNotFoundException ex)
            {
                await ReturnErrorResult(context, ex, StatusCodes.Status404NotFound);
            }
            catch (InvalidAccountException ex)
            {
                await ReturnErrorResult(context, ex, StatusCodes.Status400BadRequest);
            }
            catch (InvalidAddFundsValidationException ex)
            {
                await ReturnErrorResult(context, ex, StatusCodes.Status400BadRequest);
            }
            catch (InvalidAtmAmountException ex)
            {
                await ReturnErrorResult(context, ex, StatusCodes.Status400BadRequest);
            }
            catch (InvalidBalanceException ex)
            {
                await ReturnErrorResult(context, ex, StatusCodes.Status400BadRequest);
            }
            catch (InvalidCardException ex)
            {
                await ReturnErrorResult(context, ex, StatusCodes.Status400BadRequest);
            }
            catch (InvalidTransactionValidation ex)
            {
                await ReturnErrorResult(context, ex, StatusCodes.Status400BadRequest);
            }
            catch (UnsupportedCurrencyException ex)
            {
                await ReturnErrorResult(context, ex, StatusCodes.Status400BadRequest);
            }
            catch (UserNotFoundException ex)
            {
                await ReturnErrorResult(context, ex, StatusCodes.Status404NotFound);
            }
            catch (UserValidationException ex)
            {
                await ReturnErrorResult(context, ex, StatusCodes.Status400BadRequest);
            }
            catch (DomainException dex)
            {
                await ReturnErrorResult(context, dex, StatusCodes.Status500InternalServerError);
            }
            
        }

        private async Task ReturnErrorResult(HttpContext context, DomainException exception, int statusCode)
        {
            var errorLog = exception.LogMessage;
            _seqLogger.LogError(errorLog.Message, errorLog.Params);

            var problemDetails = new
            {
                Status = statusCode,
                Title = exception.Message,
                IsSuccess = false
            };

            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
