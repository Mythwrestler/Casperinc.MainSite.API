using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Casperinc.MainSite.API.DTOModels
{
    public class TagToCreateDTO
    {
        public TagToCreateDTO() {}

        [Required]
        public string KeyWord { get; set; }

    }
}
