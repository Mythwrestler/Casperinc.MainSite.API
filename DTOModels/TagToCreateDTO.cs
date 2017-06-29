using System;
using System.ComponentModel.DataAnnotations;

namespace Casperinc.MainSite.API.DTOModels
{
    public class TagToCreateDTO
    {
        public TagToCreateDTO() {}

        [Required]
        public Guid Id { get; set; }

        [Required]
        public string KeyWord { get; set; }
    }
}
