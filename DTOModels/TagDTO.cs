using System;
using System.ComponentModel.DataAnnotations;

namespace MainSiteCore.DTOModels
{
    public class TagDTO
    {
        public TagDTO() {}

        [Required]
        public Guid Id { get; set; }

        [Required]
        public string KeyWord { get; set; }
    }
}
