using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CasperInc.MainSite.API.DTOModels
{
    public class CommentDTO
    {
        [Required]
        public Guid GuidId { get; set;}

        [Required]
        public string Text { get; set;}

        [Required]
        public string UserId { get; set; }

    }

}