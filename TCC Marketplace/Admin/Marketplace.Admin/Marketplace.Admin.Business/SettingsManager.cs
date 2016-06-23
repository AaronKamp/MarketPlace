using System.Linq;
using Marketplace.Admin.Data.Infrastructure;
using Marketplace.Admin.Data.Repository;
using Marketplace.Admin.Model;

namespace Marketplace.Admin.Business
{
   
    public class SettingsManager : ISettingsManager
    {
        private readonly ISettingsRepository _settingsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SettingsManager(ISettingsRepository settingsRepository,IUnitOfWork unitOfWork )
        {
            _settingsRepository = settingsRepository;
            _unitOfWork = unitOfWork;
        }

        public void UpdateSettings(ConfigurationSettings settings)
        {
            var existingSetting = _settingsRepository.GetAll().FirstOrDefault();
            MapSettings(settings, existingSetting);  
            _settingsRepository.Update(existingSetting);
        }

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

        public ConfigurationSettings GetSettings()
        {
            return _settingsRepository.GetAll().FirstOrDefault();
        }

        public void SaveSettings()
        {
            _unitOfWork.Commit();
        }

        public void Create(ConfigurationSettings settings)
        {
            _settingsRepository.Add(settings);
        }
    }
}
