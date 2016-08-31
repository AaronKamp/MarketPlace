using Newtonsoft.Json;

namespace Extensions
{
    /// <summary>
    /// Extension method utility class
    /// </summary>
    internal class ExtensionUtils
    {
        /// <summary>
        /// Return JSON deserializer settings.
        /// </summary>
        public static JsonSerializerSettings GetSerializerSettings()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Formatting = Formatting.Indented;
            settings.ContractResolver = new JsonContractResolver();
            return settings;
        }
    }
}
