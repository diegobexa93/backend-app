using BaseShare.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace User.API.Middleware
{
    internal sealed class ExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var problemDetails = exception switch
            {
                NotFoundException nf => new ProblemDetails
                {
                    Title = "Not Found",
                    Detail = nf.Message,
                    Status = StatusCodes.Status404NotFound,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4" // 404 Not Found
                },
                UnauthorizedAccessException ua => new ProblemDetails
                {
                    Title = "Unauthorized",
                    Detail = ua.Message,
                    Status = StatusCodes.Status401Unauthorized,
                    Type = "https://tools.ietf.org/html/rfc7235#section-3.1" // 401 Unauthorized
                },
                ValidationException ve => new ProblemDetails
                {
                    Title = "Validation Errors",
                    Detail = ve.Message,
                    Status = StatusCodes.Status400BadRequest,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1", // 400 Bad Request
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
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1" // 400 Bad Request
                },
                Exception ex => new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = ex.Message,
                    Status = StatusCodes.Status500InternalServerError,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1" // 500 Internal Server Error
                }
            };

            httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
            httpContext.Response.ContentType = "application/problem+json";
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
