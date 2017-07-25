using System;

namespace Casperinc.MainSite.API.Data.Models
{
    // Narrative - Tag Many to Many reference
    public class NarrativeTagDataModel
    {
        public Int64 NarrativeId { get; set; }
        public NarrativeDataModel NarrativeData { get; set; }

        public Int64 TagId { get; set; }
        public TagDataModel TagData { get; set; }
    }
}