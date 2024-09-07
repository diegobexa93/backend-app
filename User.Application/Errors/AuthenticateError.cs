using BaseShare.Common.Domain;
using BaseShare.Common.Results;

namespace User.Application.Errors
{
    public static class AuthenticateError
    {
        public static readonly Error LoginEmpty = new Error(
        ErrorType.Validation, "LoginEmpty", "Login is empty!");

        public static readonly Error PasswordEmpty = new Error(
            ErrorType.Validation, "PasswordEmpty", "Password is empty!");

        public static readonly Error UserNotFound = new Error(
           ErrorType.Validation, "UserNotFound", "Username or password incorrect!");

    }
}
