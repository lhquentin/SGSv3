using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SGSv3.Models
{
    [MetadataType(typeof(ApprentissageMetaData))]
    public partial class Apprentissage
    {
        private class ApprentissageMetaData
        {
            [DisplayName("Date")]
            [Required]
            [DataType(DataType.Date)]
            public DateTime date { get; set; }

            [DisplayName("Description")]
            [Required]
            public string description { get; set; }
        }
    }
}