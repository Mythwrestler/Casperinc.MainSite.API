using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CasperInc.MainSiteCore.Data.Models
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

        public virtual List<NarrativeTagDataModel> NarrativeTags { get; set; }
    }


}
