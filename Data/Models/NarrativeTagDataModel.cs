using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasperInc.MainSite.API.Data.Models
{
    // Narrative - Tag Many to Many reference
    public class NarrativeTagDataModel
    {
        public Int64 NarrativeId { get; set; }
        public NarrativeDataModel NarrativeData { get; set; }

        public Int64 TagId { get; set; }
        public TagDataModel TagData { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedDate { get; set; }
        
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedDate { get; set; }
    }
}