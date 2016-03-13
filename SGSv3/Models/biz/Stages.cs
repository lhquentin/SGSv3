using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SGSv3.Models
{
    [MetadataType(typeof(StageMetaData))]
    public partial class Stage
    {
        private class StageMetaData
        {
            [DisplayName("Date de début")]
            [Required]
            [DataType(DataType.Date)]
            public System.DateTime dateDebut { get; set; }

            [DisplayName("Nombre de semaines")]
            [Required]
            public int nbSemaine { get; set; }

            [DisplayName("Entreprise")]
            public int noENT { get; set; }

            [DisplayName("Stagiaire")]
            [Required]
            public int noStagiaire { get; set; }

            [DisplayName("Superviseur")]
            [Required]
            public int noSuperviseur { get; set; }

            [DisplayName("Tuteur")]
            [Required]
            public int noTuteur { get; set; }

            [DisplayName("Description")]
            [Required]
            [MaxLength(500)]
            public string description { get; set; }
        }
    }
}