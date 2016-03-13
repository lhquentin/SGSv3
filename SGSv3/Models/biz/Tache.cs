using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SGSv3.Models
{
    [MetadataType(typeof(TacheMetaData))]
    public partial class Tache
    {
        private class TacheMetaData
        {
            [DisplayName("Date")]
            [Required]
            [DataType(DataType.Date)]
            public DateTime date { get; set; }

            [DisplayName("Durée (en heure)")]
            [Required]
            public int duree { get; set; }

            [DisplayName("Description")]
            [Required]
            public string description { get; set; }

        }
    }
}