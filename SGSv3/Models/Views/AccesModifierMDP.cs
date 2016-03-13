using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SGSv3.Models.Views
{
    public class AccesModifierMDP
    {
        [Required]
        [Display(Name = "Nouveau mdp")]
        [DataType(DataType.Password)]
        public string nouveauMDP { get; set; }

        [Required]
        [Display(Name = "Confirmer nouveau mdp")]
        [DataType(DataType.Password)]
        public string confirmerNouveauMDP { get; set; }
    }
}