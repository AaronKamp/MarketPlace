using Marketplace.Admin.Model;

namespace Marketplace.Admin.Business
{
    public interface ISettingsManager
    {
        void UpdateSettings(ConfigurationSettings settings);
        void SaveSettings();
        ConfigurationSettings GetSettings();
        void Create(ConfigurationSettings settings);
    }
}
