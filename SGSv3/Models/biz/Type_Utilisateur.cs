using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SGSv3.Models
{
    [MetadataType(typeof(TypeUtilisateurMetaData))]
    public partial class Type_Utilisateur
    {
        private class TypeUtilisateurMetaData
        {
            [DisplayName("Type")]
            [Required]
            public string libelleTYPE { get; set; }

        }
    }
}