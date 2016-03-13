using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SGSv3.Models
{
    [MetadataType(typeof(EntrepriseMetaData))]
    public partial class Entreprise
    {
                private class EntrepriseMetaData
                {
                    [DisplayName("Nom")]
                    [Required]
                    public string nomENT { get; set; }

                    [DisplayName("Adresse")]
         
                    public string adresseENT { get; set; }

                    [DisplayName("Ville")]
            
                    public string villeENT { get; set; }

                    [DisplayName("CP")]
                 
                    public string cpENT { get; set; }

                    [DisplayName("Téléphone")]
                 
                    public string telENT { get; set; }

                    [DisplayName("Fax")]
                  
                    public string faxENT { get; set; }

                    [DisplayName("Courriel")]
                
                    public string courrielENT { get; set; }
                }

    }

}