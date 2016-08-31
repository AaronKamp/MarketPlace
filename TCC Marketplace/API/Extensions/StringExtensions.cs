using System;
using System.Text;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Extensions
{
    /// <summary>
    /// Custom string extension methods.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Checks if a string is Base64 encoded.
        /// </summary>
        /// <param name="input"> String</param>
        /// <returns>True if string is base64. Otherwise false.</returns>
        public static bool IsBase64String(this string input)
        {
            input = input.Trim();
            return (input.Length % 4 == 0) 
                && Regex.IsMatch(input, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
        }
        /// <summary>
        /// Convert a string to base64.
        /// </summary>
        /// <param name="input"> String </param>
        /// <returns> Base64 encoded string.</returns>
        public static string ToBase64String(this string input)
        {
            var toEncodeAsBytes = Encoding.ASCII.GetBytes(input);
            var returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }
        /// <summary>
        /// Get the decoded string from Base64 encoded version.
        /// </summary>
        /// <param name="input"> String.</param>
        /// <returns> Original string.</returns>
        public static string FromBase64String(this string input)
        {
            input = input.Replace("=", "");
            int len = input.Length % 4;
            if (len > 0)
                input = input.PadRight(input.Length + (4 - len), '=');
            var encodedDataAsBytes = System.Convert.FromBase64String(input);
            var returnValue = Encoding.ASCII.GetString(encodedDataAsBytes);
            return returnValue;
        }
              
        /// <summary>
        /// Deserialize a JSON object into the corresponding object.
        /// </summary>
        /// <returns> C# object of type T. </returns>
        public static T JsonDeserializeObject<T>(this string val)
        {
            JsonSerializerSettings settings = ExtensionUtils.GetSerializerSettings();
            return (string.IsNullOrEmpty(val)) ? default(T) : JsonConvert.DeserializeObject<T>(val,settings);
        }

      
    }
}
