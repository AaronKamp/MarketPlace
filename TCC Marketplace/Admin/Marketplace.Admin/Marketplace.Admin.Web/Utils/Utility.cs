namespace Marketplace.Admin.Utils
{
    /// <summary>
    /// Houses all common utility methods.
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Truncates a string by a limit.
        /// </summary>
        /// <param name="value"> String to truncates.</param>
        /// <param name="maxChars"> Max length.</param>
        /// <returns></returns>
        public static string Truncate(this string value, int maxChars)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + " ..";
        }
    }
}