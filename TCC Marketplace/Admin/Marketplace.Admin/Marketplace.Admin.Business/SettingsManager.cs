using System.Linq;
using Marketplace.Admin.Data.Infrastructure;
using Marketplace.Admin.Data.Repository;
using Marketplace.Admin.Model;

namespace Marketplace.Admin.Business
{
   /// <summary>
   /// Handles Settings functions.
   /// </summary>
    public class SettingsManager : ISettingsManager
    {
        private readonly ISettingsRepository _settingsRepository;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Parameterized constructor to work with dependency injection
        /// </summary>
        /// <param name="settingsRepository"></param>
        /// <param name="unitOfWork"></param>
        public SettingsManager(ISettingsRepository settingsRepository,IUnitOfWork unitOfWork )
        {
            _settingsRepository = settingsRepository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Update Settings
        /// </summary>
        /// <param name="settings"></param>
        public void UpdateSettings(ConfigurationSettings settings)
        {
            var existingSetting = _settingsRepository.GetAll().FirstOrDefault();
            //Maps settings from incoming object settings to existing setting. 
            MapSettings(settings, existingSetting);  
            _settingsRepository.Update(existingSetting);
        }

        /// <summary>
        /// Maps settings from incoming object settings to existing object  existingSetting. 
        /// </summary>
        /// <param name="settings">Incoming object </param>
        /// <param name="existingSetting"> existing object.</param>
        private void MapSettings(ConfigurationSettings settings, ConfigurationSettings existingSetting)
        {
            existingSetting.FtpHostAddress = settings.FtpHostAddress;
            existingSetting.FtpPort = settings.FtpPort;
            existingSetting.FtpUser = settings.FtpUser;
            existingSetting.SshPrivateKey = settings.SshPrivateKey ?? existingSetting.SshPrivateKey;
            existingSetting.IsSshPasswordProtected = settings.IsSshPasswordProtected;
            existingSetting.SshPrivateKeyPassword = existingSetting.IsSshPasswordProtected ? settings.SshPrivateKeyPassword??existingSetting.SshPrivateKeyPassword : null ;
            existingSetting.FtpRemotePath = settings.FtpRemotePath;
            existingSetting.FromEmail = settings.FromEmail;
            existingSetting.FromEmailPassword = settings.FromEmailPassword ?? existingSetting.FromEmailPassword;
            existingSetting.ToEmails = settings.ToEmails;
            existingSetting.SmtpClient = settings.SmtpClient;
            existingSetting.SmtpPort = settings.SmtpPort;
            existingSetting.UpdatedUser = settings.UpdatedUser;
            existingSetting.UpdatedDate = settings.UpdatedDate;

        }

        /// <summary>
        /// Gets the current settings.
        /// </summary>
        /// <returns> ConfigurationSettings.</returns>
        public ConfigurationSettings GetSettings()
        {
            return _settingsRepository.GetAll().FirstOrDefault();
        }

        /// <summary>
        /// Save settings changes.
        /// </summary>
        public void SaveSettings()
        {
            _unitOfWork.Commit();
        }

        /// <summary>
        /// Add new settings.
        /// </summary>
        /// <param name="settings"></param>
        public void Create(ConfigurationSettings settings)
        {
            _settingsRepository.Add(settings);
        }
    }
}
