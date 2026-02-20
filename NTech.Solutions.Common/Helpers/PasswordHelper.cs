using NTech.Solutions.Common.Models.Database;
using System.Text.RegularExpressions;

namespace NTech.Solutions.Common.Helpers
{
    /// <summary>
    /// Provides helper methods for validating password strength according to common security requirements.
    /// </summary>
    /// <remarks>This class is intended to assist in enforcing strong password policies by checking for
    /// characteristics such as minimum length and character diversity. It does not perform checks against known
    /// compromised passwords or advanced entropy analysis.</remarks>
    public static class PasswordHelper
    {
        /// <summary>
        /// Determines whether the specified password meets common security requirements.
        /// </summary>
        /// <remarks>This method checks for a minimum password length and the presence of uppercase and
        /// lowercase letters, digits, and special characters to help enforce strong password policies. It does not
        /// check against common password lists or perform entropy analysis.</remarks>
        /// <param name="password">The password string to validate. Cannot be null or empty.</param>
        /// <returns>true if the password is at least 8 characters long, contains at least one uppercase letter, one lowercase
        /// letter, one digit, one special character, and contains no whitespace; otherwise, false.</returns>
        public static bool IsPasswordSecure(string password)
        {
            if (string.IsNullOrEmpty(password)) return false;

            // Regex to check if the password meets the following criteria:
            // ^               - Ensures the match starts at the beginning of the string.
            // (?=.*[A-Z])     - Asserts that there is at least one uppercase letter in the string.
            // (?=.*[a-z])     - Asserts that there is at least one lowercase letter in the string.
            // (?=.*\d)        - Asserts that there is at least one digit (number) in the string.
            // (?=.*[\W_])     - Asserts that there is at least one special character (non-word character or underscore).
            // [^\s]{8,}       - Ensures the string is at least 8 characters long and does not contain any whitespace.
            // $               - Ensures the match ends at the end of the string.
            var regex = new Regex(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_])[^\s]{8,}$");
            return regex.IsMatch(password);
        }
    }
}
