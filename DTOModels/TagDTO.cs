using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Casperinc.MainSite.API.DTOModels
{
    public class TagDTO
    {
        public TagDTO() {}

        [Required]
        public Guid Id { get; set; }

        [Required]
        public string KeyWord { get; set; }

        [Required]
        public IEnumerable<Guid> Narratives { get; set; }
    }
}
