using System.Text.RegularExpressions;

namespace BaseShare.Common.Utility
{
    public class EmailValidator
    { // Regular expression for validating an email address
        private static readonly Regex EmailRegex = new Regex(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false; // Email is null or empty
            }

            return EmailRegex.IsMatch(email); // Check if the email matches the regex pattern
        }
    }
}
