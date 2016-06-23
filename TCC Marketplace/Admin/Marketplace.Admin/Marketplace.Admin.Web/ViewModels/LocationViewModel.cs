using System.Collections.Generic;

namespace Marketplace.Admin.ViewModels
{
    public class LocationViewModel
    {
        public int CountryId { get; set; }

        public string CountryName { get; set; }

        public int StateId { get; set; }

        public string StateName { get; set; }

        public List<int> SCFs { get; set;}

        public string SCFNames { get; set; }     

    }
}