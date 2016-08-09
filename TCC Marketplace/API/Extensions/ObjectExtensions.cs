using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Transform object into Identity data type (integer).
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="defaultId">The default identifier.</param>
        /// <returns>Parsed integer value</returns>
        public static int AsId(this object item, int defaultId = -1)
        {
            if (item == null)
                return defaultId;

            int result;
            return !int.TryParse(item.ToString(), out result) ? defaultId : result;
        }

        /// <summary>
        /// Transform object into integer data type.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="defaultInt">The default int.</param>
        /// <returns>Parsed integer value</returns>
        public static int AsInt(this object item, int defaultInt = default(int))
        {
            if (item == null)
                return defaultInt;

            int result;
            return !int.TryParse(item.ToString(), out result) ? defaultInt : result;
        }

        /// <summary>
        /// Transform object into double data type.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="defaultDouble">The default double.</param>
        /// <returns>Parsed double value</returns>
        public static double AsDouble(this object item, double defaultDouble = default(double))
        {
            if (item == null)
                return defaultDouble;

            double result;
            return !double.TryParse(item.ToString(), out result) ? defaultDouble : result;
        }

        /// <summary>
        /// Transform object into string data type.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="defaultString">The default string.</param>
        /// <returns>Parsed string value</returns>
        public static string AsString(this object item, string defaultString = default(string))
        {
            if (item == null || item.Equals(DBNull.Value))
                return defaultString;

            return item.ToString().Trim();
        }

        /// <summary>
        /// Transform object into DateTime data type.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="defaultDateTime">The default date time.</param>
        /// <returns>Parsed value as datatime</returns>
        public static DateTime AsDateTime(this object item, DateTime defaultDateTime = default(DateTime))
        {
            if (item == null || string.IsNullOrEmpty(item.ToString()))
                return defaultDateTime;

            DateTime result;
            return !DateTime.TryParse(item.ToString(), out result) ? defaultDateTime : result;
        }

        /// <summary>
        /// Transform object into bool data type.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="defaultBool">if set to <c>true</c> [default bool].</param>
        /// <returns>Parsed value as boolean</returns>
        public static bool AsBool(this object item, bool defaultBool = default(bool))
        {
            return item == null ? defaultBool : new List<string>() { "yes", "y", "true" }.Contains(item.ToString().ToLower());
        }

        /// <summary>
        /// Transform string into byte array.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>Transforms the data into byte array</returns>
        public static byte[] AsByteArray(this string s)
        {
            return string.IsNullOrEmpty(s) ? null : Convert.FromBase64String(s);
        }

        /// <summary>
        /// Transform object into base64 string.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>Base 64 version of string</returns>
        public static string AsBase64String(this object item)
        {
            return item == null ? null : Convert.ToBase64String((byte[])item);
        }

        /// <summary>
        /// Transform object into Guid data type.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>Transforms the item to guid value</returns>
        public static Guid AsGuid(this object item)
        {
            try { return new Guid(item.ToString()); }
            catch { return Guid.Empty; }
        }

        /// <summary>
        /// Takes an enumerable source and returns a comma separate string.
        /// Handy for building SQL Statements (for example with IN () statements) from object collections
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TU"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="func">The function.</param>
        /// <returns></returns>
        public static string CommaSeparate<T, TU>(this IEnumerable<T> source, Func<T, TU> func)
        {
            return string.Join(",", source.Select(s => func(s).ToString()).ToArray());
        }

        public static string JsonSerializeObject(this object val)
        {
            JsonSerializerSettings settings = ExtensionUtils.GetSerializerSettings();
            return (val==null)?null:JsonConvert.SerializeObject(val,settings);
        }
    }
}
