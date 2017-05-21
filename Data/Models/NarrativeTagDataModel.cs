namespace CasperInc.MainSiteCore.Data.Models
{
    // Narrative - Tag Many to Many reference
    public class NarrativeTagDataModel
    {
        public int NarrativeId { get; set; }
        public NarrativeDataModel NarrativeData { get; set; }

        public string TagKeyWord { get; set; }
        public TagDataModel TagData { get; set; }
    }
}