using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CasperInc.MainSite.API.DTOModels
{
    public class NarrativeToUpdateDTO
    {
        public  NarrativeToUpdateDTO() {}
        [Required]
        public string Title { get; set;}
        [Required]
        public string Description { get; set;}
        [Required]
        public string BodyHtml { get; set;}
        public Int16 DisplaySequence { get; set; }  
        [Required]
        public List<string> Keywords { get; set; }
    }

}
