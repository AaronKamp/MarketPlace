﻿using System.ComponentModel.DataAnnotations;

namespace Marketplace.Admin.ViewModels
{
    /// <summary>
    /// Login View Model.
    /// </summary>
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
