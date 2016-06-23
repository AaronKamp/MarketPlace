using System.ComponentModel.DataAnnotations;

namespace Marketplace.Admin.ViewModels
{
    public class SMTPDetailsViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "From Email")]
        public string FromEmail { get; set; }

        [Required]
        [RegularExpression(@"(([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)(\s*,\s*|\s*$))+", 
            ErrorMessage ="Enter e-mail addresses seperated by commas")]
        [Display(Name = "To Email")]
        public string ToEmails { get; set; }

        [Required]
        [Display(Name = "SMTP Client")]
        public string SmtpClient { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string FromEmailPassword { get; set; }

        [Required]
        [Display(Name = "SMTP Port Number")]
        public int SmtpPort { get; set; }

    }
}