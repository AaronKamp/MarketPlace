using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Marketplace.Admin.ViewModels;

namespace Marketplace.Admin.Models
{
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