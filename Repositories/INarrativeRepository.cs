using System;
using System.Collections.Generic;
using Casperinc.MainSite.API.Data.Models;

namespace Casperinc.MainSite.API.Repositories
{
    public interface INarrativeRepository
    {
        bool NarrativeExists(Guid narrativeId);
        
        IEnumerable<NarrativeDataModel> GetNarrativeList();

        IEnumerable<NarrativeDataModel> GetNarrativeListWithKeyword(string keyword);

        NarrativeDataModel GetNarrative(Guid narrativeId);

        bool TagExists(string keyword);

        bool TagExists(Guid tagId);

        IEnumerable<TagDataModel> getTagsForNarrative(Guid narrativeId);

        IEnumerable<string> GetKeywordsForNarrative(Guid narrativeId);
        
        TagDataModel GetTag(string keyword);

        TagDataModel CreateTag(string keyword);

        NarrativeDataModel CreateNarrative(NarrativeDataModel narrative, List<TagDataModel> tags);

        void SaveChanges();

    }
}