using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CasperInc.MainSiteCore.Data.Models {


    public class NarrativeDataModel {

        public NarrativeDataModel () {}

        [Key]
        [RequiredAttribute]
        public int Id { get; set;}

        [RequiredAttribute]
        public string Title { get; set;}
        [RequiredAttribute]
        public string Description { get; set;}
        [RequiredAttribute]
        public string BodyHtml { get; set;}
        [RequiredAttribute]
        public DateTime CreatedOn { get; set;}
        [RequiredAttribute]
        public DateTime UpdatedOn  {get; set;}

        // Narrative tags
        public virtual List<NarrativeTagDataModel> NarrativeTags {get; set;}

    }



}