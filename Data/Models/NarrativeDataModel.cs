using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Casperinc.MainSite.API.Data.Models {


    public class NarrativeDataModel {

        public NarrativeDataModel () {}

		[Key]
		[Required]
        public Guid Id { get; set;}

		[Required]
        public string Title { get; set;}

		[Required]
        public string Description { get; set;}

		[Required]
        public string BodyHtml { get; set;}

		[Required]
        public DateTime CreatedOn { get; set;}

		[Required]
        public DateTime UpdatedOn  {get; set;}

        [Required]
        public virtual IEnumerable<NarrativeTagDataModel> NarrativeTags { get; set; }

    }



}