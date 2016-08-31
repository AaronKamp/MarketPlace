using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Marketplace.Admin.ViewModels
{
    /// <summary>
    /// FTP details ViewModel.
    /// </summary>
    public class FtpDetailsViewModel :IValidatableObject
    {
        [Required]
        [Display(Name = "FTP Host Address")]
        public string FtpHostAddress { get; set; }
        [Required]
        [Display(Name = "FTP Port Number")]
        public int FtpPort { get; set; }
        [Required]
        [Display(Name = "FTP User")]
        public string FtpUser { get; set; }
        public string ReadMode { get; set; }

        [Display(Name = "SSH Private Key")]
        public string SshPrivateKey { get; set; }

        [Required]
        public bool IsSshPasswordProtected { get; set; }
        
        [DataType(DataType.Password)]
        [Display(Name = "SSH Password ")]
        public string SshPrivateKeyPassword { get; set; }
        [Required]
        [Display(Name = "FTP Remote Path")]
        public string FtpRemotePath { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(ReadMode == "Paste" && string.IsNullOrEmpty(SshPrivateKey))
            {
                yield return new ValidationResult("Enter SSH key ");
            }
        }

    }
   
}