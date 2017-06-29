using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Casperinc.MainSite.API.Data.Models
{

    public class TagDataModel
    {

        public TagDataModel() { }

        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string KeyWord { get; set; }

		[Required]
        public DateTime CreatedDate { get; set; }

		[Required]
        public DateTime UpdatedDate { get; set; }

        [Required]
        public virtual IEnumerable<NarrativeTagDataModel> NarrativeTags { get; set; }

    }


}
