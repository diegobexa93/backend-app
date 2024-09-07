using BaseShare.Common.Domain;
using BaseShare.Common.Results;

namespace User.Application.Errors
{
    public static class UserError
    {
        public static readonly Error PersonNameEmpty = new Error(
        ErrorType.Validation, "PersonNameEmpty", "Name is empty!");

        public static readonly Error PersonEmailEmpty = new Error(
        ErrorType.Validation, "PersonEmailEmpty", "Email is empty!");

        public static readonly Error PersonEmailInvalid = new Error(
        ErrorType.Validation, "PersonEmailInvalid", "Email invalid!");

        public static readonly Error UserPasswordEmpty = new Error(
        ErrorType.Validation, "UserPasswordEmpty", "Password invalid!");

        public static readonly Error UserExists = new Error(
        ErrorType.Validation, "UserPasswordEmpty", "Existing user!");

        public static readonly Error UserNotFound = new Error(
         ErrorType.Validation, "UserNotFound", "User not found!");

        public static readonly Error HashEmpty = new Error(
           ErrorType.Validation, "HashEmpty", "Hash is empty!");
    }
}
