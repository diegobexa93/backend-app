using BaseShare.Common.Domain;
using BaseShare.Common.Exceptions;
using BaseShare.Common.Results;

namespace User.API.Helper
{
    public static class ApiResults
    {
        public static IResult Problem(Result result)
        {
            if (result.IsSuccess)
            {
                throw new InvalidOperationException();
            }

            return Results.Problem
                (
                  title: GetTitle(result.Error),
                  detail: GetDetail(result.Error),
                  type: GetType(result.Error.Type),
                  statusCode: GetStatusCode(result.Error.Type),
                  extensions: GetErrors(result)

                );

            static string GetTitle(Error error) =>
               error.Type switch
               {
                   ErrorType.Validation => error.Code,
                   ErrorType.NotFound => error.Code,
                   ErrorType.Conflict => error.Code,
                   ErrorType.Problem => error.Code,
                   _ => "Server Failure"
               };

            static string GetDetail(Error error) =>
              error.Type switch
              {
                  ErrorType.Validation => error.Description,
                  ErrorType.NotFound => error.Description,
                  ErrorType.Conflict => error.Description,
                  ErrorType.Problem => error.Description,
                  _ => "Server Failure"
              };

            static string GetType(ErrorType? type) =>
            type switch
            {
                ErrorType.Validation => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                ErrorType.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                ErrorType.Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
                ErrorType.Problem => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                _ => "Server Failure"
            };

            static int GetStatusCode(ErrorType? type) =>
             type switch
             {
                 ErrorType.Validation => StatusCodes.Status400BadRequest,
                 ErrorType.NotFound => StatusCodes.Status404NotFound,
                 ErrorType.Conflict => StatusCodes.Status409Conflict,
                 ErrorType.Problem => StatusCodes.Status400BadRequest,
                 _ => StatusCodes.Status500InternalServerError
             };

            static Dictionary<string, object?>? GetErrors(Result result)
            {
                if (result.Error is not ValidationErrors validationErrors)
                {
                    return null;
                }

                return new Dictionary<string, object?>
                {
                    {"errors", validationErrors.Errors}
                };
            }

        }
    }
}
