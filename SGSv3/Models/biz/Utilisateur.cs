using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SGSv3.Models
{
    [MetadataType(typeof(UtilisateurMetaData))]
    public partial class Utilisateur
    {
        public string nomPrenom { get { return nomUTIL + ", " + prenomUTIL; } }
        private class UtilisateurMetaData
        {
            [DisplayName("Nom")]
            [Required]
            [MaxLength(40, ErrorMessage = "Le champ Nom doit être composé d'un maximum de 40 caractères.")]
            public string nomUTIL { get; set; }

            [DisplayName("Prenom")]
            [Required]
            [MaxLength(40, ErrorMessage = "Le champ Nom doit être composé d'un maximum de 40 caractères.")]
            public string prenomUTIL { get; set; }

            [DisplayName("Courriel")]
            [EmailAddress]
            [Required]
            public string courrielUTIL { get; set; }

            [DisplayName("Téléphone")]
            [Phone]
            public string telUTIL { get; set; }

            [DisplayName("NumeroType")]
            [Required]
            public int noTYPE { get; set; }

        }

    }
}