using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Casperinc.MainSite.API.DTOModels
{
    public class TagToUpdateDTO
    {
        public TagToUpdateDTO() {}

		[Required]
		public Guid Id { get; set; }

        [Required]
        public string KeyWord { get; set; }

    }
}
