using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectBear.Web.Models
{
    public class ExternalLoginViewModels
    {
        public string ReturnUrl { get; set; }
    }

    public class ExternalLoginConfirmationViewModel
    {
        public bool isTrue => true;

        [Required]
        [Display(Name = "I have read and understand the rules")]
        [Compare("isTrue", ErrorMessage = "Please confirm that you have read the rules.")]
        public bool RulesAccepted { get; set; }
    }
}