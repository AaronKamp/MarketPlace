using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Admin.Core.DTO
{
    public class ServiceDTO
    {
        private List<int> _locations;
        private List<int> _products;
        public int Id { get; set; }
        public byte ServiceTypeId { get; set; }
        public string Tilte { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string URL { get; set; }
        public string PartnerPromoCode { get; set; }
        public bool IsActive { get; set; }
        public string IconImage { get; set; }
        public string SliderImage { get; set; }
        public string CustomField1 { get; set; }
        public string CustomField2 { get; set; }
        public string CustomField3 { get; set; }
        public bool MakeLive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public string InAppPurchaseId { get; set; }
        public decimal PurchasePrice { get; set; }
        public bool ServiceStatusAPIAvailable { get; set; }
        public string ServiceProviderId { get; set; }

        public List<int> Locations {
            get
            {
                if (_locations == null)
                {
                    _locations = new List<int>();
                }
                return _locations;
            }
            set
            {
                _locations = value;
            }
        }

        public List<int> Products
        {
            get
            {
                if (_products == null)
                {
                    _products = new List<int>();
                }
                return _products;
            }
            set
            {
                _products = value;
            }
        }

        public string ZipCodes { get; set; }
        public bool DisableAPIAvailable { get; set; }

    }
}
