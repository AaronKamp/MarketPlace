using Marketplace.Admin.ViewModels;

namespace Marketplace.Admin.Models
{
    /// <summary>
    /// Settings ViewModel.
    /// </summary>
    public class SettingsViewModel
    {
        public SettingsViewModel()
        {
            FtpDetails = new FtpDetailsViewModel();
            SmtpDetails = new SMTPDetailsViewModel();
        }
        public int Id { get; set; }
        public FtpDetailsViewModel FtpDetails { get; set; }
        public SMTPDetailsViewModel SmtpDetails { get; set; }
    }
    
}