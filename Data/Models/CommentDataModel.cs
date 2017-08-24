using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Casperinc.MainSite.API.Data.Models
{
    public class CommentDataModel
    {
        [Key]
        [Required]
        public Int64 UniqueId { get; set;}

        [Required]
        public Guid GuidId { get; set;}

        [Required]
        public string Text { get; set;}

        [Required]
        public string UserId { get; set; }
        [Required]
        public Int64 NarrativeId { get; set; }

		[Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set;}

		[Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedOn  {get; set;}

        public Int64? ParentId { get; set; }

        [ForeignKey("NarrativeId")]
        public virtual NarrativeDataModel Narrative { get; set; }

        // [ForeignKey("UserId")]
        // public virtual UserDataModel Author { get; set; }

        [ForeignKey("ParentId")]
        public virtual CommentDataModel Parent { get; set; }

        public virtual IEnumerable<CommentDataModel> Children { get; set; }

    }
}