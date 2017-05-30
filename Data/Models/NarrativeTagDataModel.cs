using System;

namespace CasperInc.MainSiteCore.Data.Models
{
    // Narrative - Tag Many to Many reference
    public class NarrativeTagDataModel
    {
        public Guid NarrativeId { get; set; }
        public NarrativeDataModel NarrativeData { get; set; }

        public Guid TagId { get; set; }
        public TagDataModel TagData { get; set; }
    }
}