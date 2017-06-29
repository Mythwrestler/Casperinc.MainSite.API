using System;
namespace Casperinc.MainSite.API.DTOModels
{
    public class narativeTags
    {
        public narativeTags(){ }

        public Guid NarrativeId { get; set; }
        public NarrativeDTO NarrativeData { get; set; }

        public Guid TagId { get; set; }
        public TagDTO TagData { get; set; }

    }
}
