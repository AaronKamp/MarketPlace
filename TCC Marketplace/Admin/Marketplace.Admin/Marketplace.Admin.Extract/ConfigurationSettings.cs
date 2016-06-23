using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Admin.Extract
{
    public class ConfigurationSettings
    {
        public int Id { get; set; }
        public string FtpHostAddress { get; set; }
        public int FtpPort { get; set; }
        public string FtpUser { get; set; }
        public string SshPrivateKey { get; set; }
        public bool IsSshPasswordProtected { get; set; }
        public string SshPrivateKeyPassword { get; set; }
        public string FtpRemotePath { get; set; }
        public string FromEmail { get; set; }
        public string FromEmailPassword { get; set; }
        public string ToEmails { get; set; }
        public string SmtpClient { get; set; }
        public int SmtpPort { get; set; }
    }
}
