using Marketplace.Admin.Model;

namespace Marketplace.Admin.Business
{
    public interface ISettingsManager
    {
        /// <summary>
        /// Update Settings
        /// </summary>
        void UpdateSettings(ConfigurationSettings settings);

        /// <summary>
        /// Save settings changes.
        /// </summary>
        void SaveSettings();

        /// <summary>
        /// Gets the current settings.
        /// </summary>
        ConfigurationSettings GetSettings();

        /// <summary>
        /// Add new settings.
        /// </summary>
        void Create(ConfigurationSettings settings);
    }
}
