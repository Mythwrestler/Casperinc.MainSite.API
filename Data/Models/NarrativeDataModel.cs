using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Casperinc.MainSite.Helpers;

namespace Casperinc.MainSite.API.Data.Models {


    public class NarrativeDataModel {

        public NarrativeDataModel () {}

		[Key]
        [Required]
        public Int64 UniqueId { get; set; }

		[Required]
        public Guid GuidId { get; set;}

        [Required]
        public NarrativeTypes Type { get; set;}

		[Required]
        public string Title { get; set;}

		[Required]
        public string Description { get; set;}

		[Required]
        public string BodyHtml { get; set;}

		[Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set;}

		[Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedOn  {get; set;}

        public Int16? DisplaySequence { get; set; } 
        
        [Required]
        public string UserId { get; set; }    

        [Required]
        public virtual IEnumerable<NarrativeTagDataModel> NarrativeTags { get; set; }

        //[ForeignKey("UserId")]
        //public virtual UserDataModel Author { get; set; }

        public virtual List<CommentDataModel> Comments { get; set; }

    }

}