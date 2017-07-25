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
        public Int64 UniqueId { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string KeyWord { get; set; }

		[Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedDate { get; set; }

		[Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedDate { get; set; }

        [Required]
        public virtual IEnumerable<NarrativeTagDataModel> NarrativeTags { get; set; }

    }


}
