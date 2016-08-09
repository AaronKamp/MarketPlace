using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Marketplace.Admin.Models;

namespace Marketplace.Admin.ViewModels
{
    public class ServiceViewModel : IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        public byte ServiceTypeId { get; set; }

        [Required]
        [StringLength(25)]
        public string Tilte { get; set; }

        [DisplayName("Short description")]
        [Required]
        [StringLength(130)]
        public string ShortDescription { get; set; }

        [DisplayName("Description")]
        [Required]
        [StringLength(250)]
        public string LongDescription { get; set; }

        [DisplayName("Start date")]
        [Required]
        public System.DateTime StartDate { get; set; }

        [DisplayName("End date")]
        [Required]
        public System.DateTime EndDate { get; set; }

        [Required]
        [Url]
        [DisplayName("Third party sign-up URL")]
        public string URL { get; set; }

        [Required]
        [DisplayName("Partner promo code")]
        public string PartnerPromoCode { get; set; }

        [Required]
        [DisplayName("Is active")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Please select an icon image")]
        [DisplayName("Icon image")]
        public string IconImage { get; set; }

        [DisplayName("Slider image")]
        public string SliderImage { get; set; }

        [DisplayName("Custom 1")]
        public string CustomField1 { get; set; }

        [DisplayName("Custom 2")]
        public string CustomField2 { get; set; }

        [DisplayName("Custom 3")]
        public string CustomField3 { get; set; }

        [DisplayName("Make live")]
        public bool MakeLive { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public string UpdatedUser { get; set; }

        [DisplayName("In-app purchase ID")]
        public string InAppPurchaseId { get; set; }

        [Required]
        [DisplayName("Price")]
        public decimal PurchasePrice { get; set; }

        [Required]
        [DisplayName("Service status API available")]
        public bool ServiceStatusAPIAvailable { get; set; }

        [Required]
        [DisplayName("Service Provider Name")]
        public string ServiceProviderId { get; set; }

        [DisplayName("Service Locations")]
        public List<LocationViewModel> Locations { get; set; }

        [DisplayName("Service Products")]
        public List<ProductViewModel> Products { get; set; }

        [DisplayName("Zip Codes")]
        public string ZipCodes { get; set; }

        [Required]
        [DisplayName("Service disable API available")]
        public bool DisableAPIAvailable { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (MakeLive && string.IsNullOrEmpty(InAppPurchaseId))
            {
                yield return new ValidationResult("In-app purchase ID is required when Make live value is 'Yes'.");
            }
            else if (StartDate.Date >= EndDate.Date)
            {
                yield return new ValidationResult("End date must be greater than start date.");
            }
        }
    }
}