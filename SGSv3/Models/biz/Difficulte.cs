using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SGSv3.Models
{
    [MetadataType(typeof(DifficulteMetaData))]
    public partial class Difficulte
    {
        private class DifficulteMetaData
        {
            [DisplayName("Description")]
            [Required]
            public string description { get; set; }

            [DisplayName("Moyen")]
            [Required]
            public string moyen { get; set; }

            [DisplayName("Résultat")]
            public string resultat { get; set; }
        }
    }
}