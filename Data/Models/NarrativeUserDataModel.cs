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
    public class NarrativeUserDataModel
    {
        [Key]
        [Required]
        public Int64 UniqueId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public UserDataModel UserData { get; set; }

        [Required]
        public Int64 NarrativeId { get; set; }

        [Required]
        public NarrativeDataModel NarrativeData { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedDate { get; set; }
        
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedDate { get; set; }
    }
}