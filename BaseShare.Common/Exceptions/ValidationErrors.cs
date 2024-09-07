using BaseShare.Common.Domain;
using BaseShare.Common.Results;

namespace BaseShare.Common.Exceptions
{
    public class ValidationErrors : Error
    {
        public Error[] Errors { get; }

        public ValidationErrors(Error[] errors)
            : base(ErrorType.Validation, string.Empty, "Validation errors occurred.")
        {
            Errors = errors ?? Array.Empty<Error>();
        }
    }
}
