using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Marketplace.Admin.ViewModels
{
    public class ServiceProviderViewModel : IValidatableObject
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name ="Service provider name")]
        public string Name { get; set; }

        [Required]
        [Display(Name ="Sign-up URL")]
        [Url]
        public string SignUpUrl { get; set; }

        [Display(Name ="Status API URL")]
        [Url]
        public string StatusUrl { get; set; }

        [Display(Name ="Unenroll API URL")]
        [Url]
        public string UnEnrollUrl { get; set; }

        [Required]
        [Display(Name ="Generate bearer token")]
        public bool GenerateBearerToken { get; set; }

        [Required]
        [Display(Name ="Is active")]
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        [Display(Name ="Token API URL")]
        [Url]
        public string TokenUrl { get; set; }

        [Display(Name ="App ID")]
        public string AppId { get; set; }

        [Display(Name ="Secret key")]
        public string SecretKey { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedUser { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(TokenUrl) && GenerateBearerToken)
                yield return new ValidationResult("Token URL required", new List<string> { "TokenUrl" } );
            if (string.IsNullOrWhiteSpace(AppId) && GenerateBearerToken)
                yield return new ValidationResult("App ID required", new List<string> { "AppId" });
            if (string.IsNullOrWhiteSpace(SecretKey) && GenerateBearerToken)
                yield return new ValidationResult("Secret key required", new List<string> { "SecretKey" });
        }
    }

}