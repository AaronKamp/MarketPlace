﻿using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Marketplace.Admin.ViewModels
{
    /// <summary>
    /// Service Grid Pagination Model.
    /// </summary>
    public class ServiceGridPaginationModel : PaginationViewModel
    {
        public List<ServiceGridViewModel> Services { get; set; }
     
    }

    /// <summary>
    /// ServiceGrid View Model.
    /// </summary>
    public class ServiceGridViewModel
    {
        public int RowNo { get; set; }

        [DisplayName("Service ID")]
        public int ServiceId { get; set; }

        public string Title { get; set; }

        [DisplayName("Short Description")]
        public string ShortDescription { get; set; }

        public string Type { get; set; }

        [DisplayName("Product ID")]
        public string ProductId { get; set; }

        public string Countries { get; set; }

        public string States { get; set; }

        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }

        [DisplayName("End Date")]
        public DateTime EndDate { get; set; }      

        public bool IsActive { get; set; }

        public string IconImage { get; set; }
    }
   
}