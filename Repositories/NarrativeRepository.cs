using System;
using System.Collections.Generic;
using System.Linq;
using CasperInc.MainSiteCore.Data;
using CasperInc.MainSiteCore.Data.Models;

namespace CasperInc.MainSiteCore.Repositories
{
    public class NarrativeRepository : INarrativeRepository
    {

        private MainSiteCoreDBContext _dbContext;

        public NarrativeRepository(MainSiteCoreDBContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        public IEnumerable<NarrativeDataModel> GetNarrativeListWithKeyword(string keyword)
        {
            var narrativeListFromDb =
                _dbContext.NarrativeTagCrossWalk
                          .Where(c => c.TagData.KeyWord == keyword)
                          .Select(j => j.NarrativeData)
                          .ToList();


            return narrativeListFromDb;

        }



        public IEnumerable<NarrativeDataModel> GetNarrativeList()
        {
            var narrativeListFromDb = _dbContext.NarrativeData.AsEnumerable();

            foreach (var narrative in narrativeListFromDb)
            {
                narrative.NarrativeTags = _dbContext.NarrativeTagCrossWalk
                                     .Where(c => c.NarrativeId == narrative.Id)
                                     .ToList();
            }

            return narrativeListFromDb;
        }

        public NarrativeDataModel GetNarrative(Guid narrativeId)
        {
            return _dbContext.NarrativeData.Where(n => n.Id == narrativeId).FirstOrDefault();
        }

        public IEnumerable<TagDataModel> getTagsForNarrative(Guid narrativeId)
        {
            var tags = _dbContext.NarrativeTagCrossWalk
                                 .Where(n => n.NarrativeId == narrativeId)
                                 .Select(c => c.TagData)
                                 .ToList();
            return tags;

        }



		public bool TagExists(string keyword)
        {
            return _dbContext.TagData.Any(t => t.KeyWord == keyword);
		}

		public bool TagExists(Guid tagId)
		{
			return _dbContext.TagData.Any(t => t.Id == tagId);
		}




		public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

    }
}