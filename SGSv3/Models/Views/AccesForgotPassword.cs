using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SGSv3.Models.Views
{
    public class AccesForgotPassword
    {
        [Required]
        [Display(Name = "Adresse courriel")]
        [DataType(DataType.EmailAddress)]
        public string courriel { get; set; }
    }
}