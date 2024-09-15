using BaseShare.Common.Exceptions;
using BaseShare.Common.Interface.Communication;
using EventBusRabbitMQ.Events;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace User.API.Middleware
{
    internal sealed class ExceptionHandler(IServiceProvider serviceProvider) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            // Map the exception to ProblemDetails
            var problemDetails = MapToProblemDetails(exception);

            // Create a log entry based on the exception
            var log = new LogExceptionsEvent
            {
                Title = problemDetails.Title,
                Detail = problemDetails.Detail,
                StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError,
                Type = problemDetails.Type,
                Extensions = problemDetails.Extensions
            };

            try
            {
                // Resolve the IMessageBrokerLog service and send the log to another API
                var messageBrokerLog = serviceProvider.GetRequiredService<IMessageBrokerLog>();
                _= messageBrokerLog.CreateLogUserAPI(log);
            }
            catch (Exception logEx)
            {
                Console.WriteLine($"Failed to log exception: {logEx.Message}");
            }

            // Set the response details
            httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
            httpContext.Response.ContentType = "application/problem+json";
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }


        // Helper method to map exceptions to ProblemDetails
        private static ProblemDetails MapToProblemDetails(Exception exception)
        {
            return exception switch
            {
                NotFoundException nf => new ProblemDetails
                {
                    Title = "Not Found",
                    Detail = nf.Message,
                    Status = StatusCodes.Status404NotFound,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4"
                },
                UnauthorizedAccessException ua => new ProblemDetails
                {
                    Title = "Unauthorized",
                    Detail = ua.Message,
                    Status = StatusCodes.Status401Unauthorized,
                    Type = "https://tools.ietf.org/html/rfc7235#section-3.1"
                },
                ValidationException ve => new ProblemDetails
                {
                    Title = "Validation Errors",
                    Detail = ve.Message,
                    Status = StatusCodes.Status400BadRequest,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Extensions = new Dictionary<string, object?>
                {
                    { "errors", ve.Errors }
                }
                },
                ArgumentNullException an => new ProblemDetails
                {
                    Title = "Bad Request",
                    Detail = an.Message,
                    Status = StatusCodes.Status400BadRequest,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
                },
                Exception ex => new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = ex.Message,
                    Status = StatusCodes.Status500InternalServerError,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
                }
            };
        }
    }
}
