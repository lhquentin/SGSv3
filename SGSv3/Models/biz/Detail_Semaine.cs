using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SGSv3.Models
{
    [MetadataType(typeof(DetailSemaineMetaData))]

    public partial class Detail_Semaine
    {
        private class DetailSemaineMetaData
        {
            [Required]
            public int noSTAGE { get; set; }

            [DisplayName("Numéro de la semaine")]
            [Required]
            public int noSemaine { get; set; }

            [DisplayName("Synthèse de la semaine :")]
            [DataType(DataType.MultilineText)]
            [MaxLength(500, ErrorMessage = "Le champ Synthèse de la semaine doit être composé d'un maximum de 500 caractères.")]
            public string synthese { get; set; }

            [DisplayName("Horaire de la semaine :")]
            [MaxLength(500, ErrorMessage = "Le champ Horaire de la semaine doit être composé d'un maximum de 500 caractères.")]
            public string horaire { get; set; }

            [DisplayName("Environnement matériel :")]
            [MaxLength(500, ErrorMessage = "Le champ Environnement matériel doit être composé d'un maximum de 500 caractères.")]
            public string envMateriel { get; set; }

            [DisplayName("Environnement logiciel :")]
            [MaxLength(500, ErrorMessage = "Le champ Environnement logiciel doit être composé d'un maximum de 500 caractères.")]
            public string envLogiciel { get; set; }

            [DisplayName("Commentaire :")]
            public string commSuperviseur { get; set; }

            [DisplayName("Commentaire :")]
            public string commTuteur { get; set; }

            [DisplayName("Étudiant")]
            [Required]
            public bool soumisCahier { get; set; }

            [DisplayName("Superviseur")]
            [Required]
            public bool soumisSuperviseur { get; set; }

            [DisplayName("Tuteur")]
            [Required]
            public bool soumisTuteur { get; set; }

            [DisplayName("Date du début")]
            [Required]
            [DataType(DataType.Date)]
            public DateTime dateDebut { get; set; }
        }
    }
}