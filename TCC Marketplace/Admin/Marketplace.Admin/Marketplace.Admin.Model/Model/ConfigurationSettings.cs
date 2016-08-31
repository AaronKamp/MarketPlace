using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Marketplace.Admin.Model
{
    /// <summary>
    /// Configuration settings model.
    /// </summary>
    [Table("ConfigurationSettings")]
    public partial class ConfigurationSettings
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public DateTime UpdatedDate { get; set; }
        [Required]
        public string UpdatedUser { get; set; }
        [Required]
        public string FtpHostAddress { get; set; }
        [Required]
        public int FtpPort { get; set; }
        [Required]
        public string FtpUser { get; set; }
        [Required]
        public string SshPrivateKey { get; set; }
        [Required]
        public bool IsSshPasswordProtected { get; set; }
        public string SshPrivateKeyPassword { get; set; }
        [Required]
        public string FtpRemotePath { get; set; }
        [Required]
        public string FromEmail { get; set; }
        [Required]
        public string FromEmailPassword { get; set; }
        [Required]
        public string ToEmails { get; set; }
        [Required]
        public string SmtpClient { get; set; }
        [Required]
        public int SmtpPort { get; set; }


    }
}
