using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MainSiteCore.DTOModels
{
    public class NarrativeDTO
    {
        public  NarrativeDTO() {}

        [Required]
        public Guid Id { get; set;}
        [Required]
        public string Title { get; set;}
        [Required]
        public string Description { get; set;}
        [Required]
        public string BodyHtml { get; set;}
        [Required]
        public List<TagDTO> Tags { get; set; }
    }

}
