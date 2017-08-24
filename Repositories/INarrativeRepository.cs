using System;
using System.Collections.Generic;
using Casperinc.MainSite.API.Data.Models;
using Casperinc.MainSite.Helpers;

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

        List<TagDataModel> GetTags();

        List<TagDataModel> GetTagsForKeywords(List<string> keywords);

        IEnumerable<string> GetKeywordsForNarrative(Guid narrativeId);
        
        TagDataModel GetTag(string keyword);

        TagDataModel CreateTag(string keyword);

        NarrativeDataModel CreateNarrative(NarrativeDataModel narrative, IEnumerable<TagDataModel> tags);

        NarrativeDataModel UpdateNarrative(NarrativeDataModel narrative, IEnumerable<TagDataModel> tags);

        void DeleteNarrative(NarrativeDataModel narrative);

        bool SaveChanges();

    }
}