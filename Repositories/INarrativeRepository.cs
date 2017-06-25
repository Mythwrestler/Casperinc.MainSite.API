using System;
using System.Collections.Generic;
using CasperInc.MainSiteCore.Data.Models;

namespace CasperInc.MainSiteCore.Repositories
{
    public interface INarrativeRepository
    {
        IEnumerable<NarrativeDataModel> GetNarrativeList();

        IEnumerable<NarrativeDataModel> GetNarrativeListWithKeyword(string keyword);

        NarrativeDataModel GetNarrative(Guid narrativeId);

        bool TagExists(string keyword);

        bool TagExists(Guid tagId);

        IEnumerable<TagDataModel> getTagsForNarrative(Guid narrativeId);

        void SaveChanges();

    }
}