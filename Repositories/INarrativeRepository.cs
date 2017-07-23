using System;
using System.Collections.Generic;
using Casperinc.MainSite.API.Data.Models;
using CasperInc.MainSite.Helpers;

namespace Casperinc.MainSite.API.Repositories
{
    public interface INarrativeRepository
    {
        bool NarrativeExists(Guid narrativeId);
        
        PagedList<NarrativeDataModel> GetNarrativeList(NarrativeResourceParameters parms);

        IEnumerable<NarrativeDataModel> GetNarrativeListWithKeyword(string keyword);

        NarrativeDataModel GetNarrative(Guid narrativeId);

        bool TagExists(string keyword);

        bool TagExists(Guid tagId);

        IEnumerable<TagDataModel> GetTagsForNarrative(Guid narrativeId);

        IEnumerable<TagDataModel> GetTags();

        IEnumerable<string> GetKeywordsForNarrative(Guid narrativeId);
        
        TagDataModel GetTag(string keyword);

        TagDataModel CreateTag(string keyword);

        NarrativeDataModel CreateNarrative(NarrativeDataModel narrative, List<TagDataModel> tags);

        NarrativeDataModel UpdateNarrative(NarrativeDataModel narrative, List<TagDataModel> tags);

        void SaveChanges();

    }
}