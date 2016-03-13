using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SGSv3.Models.Views
{
    public class AccesConnexion
    {
        [Required]
        [Display(Name = "Adresse courriel")]
        [DataType(DataType.EmailAddress)]
        public string courriel { get; set; }

        [Required]
        [Display(Name = "Mot de passe")]
        [DataType(DataType.Password)]
        public string mdp { get; set; }

        [Display(Name = "Se souvenir de moi")]
        public bool sesouvenirdemoi { get; set; }
    }
}