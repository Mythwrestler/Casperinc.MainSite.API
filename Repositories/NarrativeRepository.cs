using System;
using System.Collections.Generic;
using System.Linq;
using Casperinc.MainSite.API.Data;
using Casperinc.MainSite.API.Data.Models;
using CasperInc.MainSite.Helpers;

namespace Casperinc.MainSite.API.Repositories
{
    public class NarrativeRepository : INarrativeRepository
    {

        private MainSiteDbContext _dbContext;



        public NarrativeRepository(MainSiteDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }



        public IEnumerable<NarrativeDataModel> GetNarrativeListWithKeyword(string keyword)
        {
            var narrativeListFromDb =
                _dbContext.NarrativeTagCrossWalk
                          .Where(c => c.TagData.KeyWord == keyword)
                          .Select(j => j.NarrativeData)
                          .AsEnumerable();


            return narrativeListFromDb;

        }



        public bool NarrativeExists(Guid narrativeId)
        {
            return _dbContext.NarrativeData.Any(n => n.Id == narrativeId);
        }



        public PagedList<NarrativeDataModel> GetNarrativeList(NarrativeResourceParameters parms)
        {

            IQueryable<NarrativeDataModel> narrativesBeforePaging;

            if (!string.IsNullOrEmpty(parms.KeywordFilter))
            {
                var filterString = parms.KeywordFilter.Trim().ToLowerInvariant();

                narrativesBeforePaging = _dbContext.NarrativeTagCrossWalk
                                                .Where(t => t.TagData.KeyWord == filterString)
                                                .Select(n => n.NarrativeData)
                                                .OrderBy(n => n.CreatedOn)
                                                .AsQueryable();
            } else {

                narrativesBeforePaging = _dbContext.NarrativeData
                                                .OrderBy(n => n.CreatedOn)
                                                .AsQueryable();
            }

            
            return PagedList<NarrativeDataModel>.Create(
                narrativesBeforePaging,
                parms.PageNumber,
                parms.PageSize
            );

        }



        public NarrativeDataModel GetNarrative(Guid narrativeId)
        {
            return _dbContext.NarrativeData.Where(n => n.Id == narrativeId).FirstOrDefault();
        }



        public IEnumerable<TagDataModel> GetTagsForNarrative(Guid narrativeId)
        {
            var tags = _dbContext.NarrativeTagCrossWalk
                                 .Where(n => n.NarrativeId == narrativeId)
                                 .Select(c => c.TagData)
                                 .ToList();
            return tags;

        }



        public IEnumerable<string> GetKeywordsForNarrative(Guid narrativeId)
        {
            return GetTagsForNarrative(narrativeId).Select(t => t.KeyWord);
        }



        public IEnumerable<TagDataModel> GetTags()
        {
            return _dbContext.TagData.AsEnumerable();
        }



		public bool TagExists(string keyword)
        {
            return _dbContext.TagData.Any(t => t.KeyWord == keyword);
		}



		public bool TagExists(Guid tagId)
		{
			return _dbContext.TagData.Any(t => t.Id == tagId);
		}



        public TagDataModel CreateTag(string keyword)
        {
            if(!TagExists(keyword))
            {

            _dbContext.TagData.Add(
                 new TagDataModel()
                {
                    KeyWord = keyword
                }
            );
            
            SaveChanges();
            }

            return _dbContext.TagData.Where(t => t.KeyWord == keyword).FirstOrDefault();
        }



		public TagDataModel GetTag(Guid tagId)
		{
			return _dbContext.TagData.Where(t => t.Id == tagId).FirstOrDefault();
		}



        public TagDataModel GetTag(string keyword)
        {
            return _dbContext.TagData.Where(t => t.KeyWord == keyword).FirstOrDefault();
        }




        public NarrativeDataModel CreateNarrative(NarrativeDataModel narrative, List<TagDataModel> tags)
        {

            foreach (TagDataModel tag in tags)
			{
				var narrativeTag = new NarrativeTagDataModel()
				{
					NarrativeId = narrative.Id,
					NarrativeData = narrative,
					TagId = tag.Id,
					TagData = tag
				};
				_dbContext.Add(narrative);
				_dbContext.Add(narrativeTag);
			}

            SaveChanges();

            return narrative;
        }



		public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

    }
}