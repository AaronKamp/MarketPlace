using System;
using System.Text;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Extensions
{
    public static class StringExtensions
    {
        public static bool IsBase64String(this string input)
        {
            input = input.Trim();
            return (input.Length % 4 == 0) 
                && Regex.IsMatch(input, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
        }

        public static string ToBase64String(this String input)
        {
            var toEncodeAsBytes = Encoding.ASCII.GetBytes(input);
            var returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        public static string FromBase64String(this String input)
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
        /// Concatenates SQL and ORDER BY clauses into a single string
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="sortExpression">The sort expression.</param>
        /// <returns>Concatenated string</returns>
        public static string OrderBy(this string sql, string sortExpression)
        {
            if (string.IsNullOrEmpty(sortExpression))
                return sql;

            return sql + " ORDER BY " + sortExpression;
        }

        public static T JsonDeserializeObject<T>(this string val)
        {
            JsonSerializerSettings settings = ExtensionUtils.GetSerializerSettings();
            return (string.IsNullOrEmpty(val)) ? default(T) : JsonConvert.DeserializeObject<T>(val,settings);
        }

      
    }
}
