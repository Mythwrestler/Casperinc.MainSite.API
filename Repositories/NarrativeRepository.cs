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
            }
            else
            {

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



        public List<TagDataModel> GetTags()
        {
            return _dbContext.TagData.AsEnumerable().ToList();
        }


        public List<TagDataModel> GetTagsForKeywords(List<string> keywords)
        {
            return _dbContext.TagData.Where(t => keywords.Contains(t.KeyWord)).AsEnumerable().ToList();
        }



        public bool TagExists(string keyword)
        {
            return _dbContext.TagData.Select(t => t.KeyWord.ToUpperInvariant()).Contains(keyword.ToUpperInvariant());
        }



        public bool TagExists(Guid tagId)
        {
            return _dbContext.TagData.Select(t => t.Id).Contains(tagId);
        }



        public TagDataModel CreateTag(string keyword)
        {
            if (!TagExists(keyword))
            {

                _dbContext.TagData.Add(
                     new TagDataModel()
                     {
                         KeyWord = keyword
                     }
                );

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




        public NarrativeDataModel CreateNarrative(NarrativeDataModel narrative, IEnumerable<TagDataModel> tags)
        {

            _dbContext.Add(narrative);
            UpdateNarrativeTagDatCrossWalk(narrative, tags);

            return narrative;
        }



        public NarrativeDataModel UpdateNarrative(NarrativeDataModel narrative, IEnumerable<TagDataModel> tags)
        {

            UpdateNarrativeTagDatCrossWalk(narrative, tags);

            return narrative;
        }

        public void DeleteNarrative(NarrativeDataModel narrative)
        {
            // remove narrativetag crosswalk entry
            
            var narrativeTagsToRemove = _dbContext.NarrativeTagCrossWalk
            .Where(nt => nt.NarrativeId == narrative.Id)
            .ToList();

            if(narrativeTagsToRemove.Count != 0) {
                _dbContext.NarrativeTagCrossWalk.RemoveRange(narrativeTagsToRemove);
            }

            _dbContext.NarrativeData.Remove(narrative);

        }


        public bool SaveChanges()
        {
            return(_dbContext.SaveChanges() >= 0);
        }


        private void UpdateNarrativeTagDatCrossWalk(NarrativeDataModel narrative, IEnumerable<TagDataModel> tags)
        {


            // remove unsued crosswalks
            var narrativeTagsToRemove = _dbContext.NarrativeTagCrossWalk
            .Where(nt => nt.NarrativeId == narrative.Id && tags.Select(t => t.Id).Contains(nt.TagId) == false )
            .ToList();


            if(narrativeTagsToRemove.Count != 0) {
                _dbContext.NarrativeTagCrossWalk.RemoveRange(narrativeTagsToRemove);
            }


            // Add new & update existing crosswalks
            foreach (var tag in tags)
            {
                // attempt to get existing narrativeTag crosswalk entry
                var narrativeTag = _dbContext.NarrativeTagCrossWalk
                    .Where(nt => nt.NarrativeId == narrative.Id && nt.TagId == tag.Id)
                    .FirstOrDefault();

                // if there is not one, remove it.
                if (narrativeTag == null)
                {
                    narrativeTag = new NarrativeTagDataModel()
                    {
                        NarrativeId = narrative.Id,
                        NarrativeData = narrative,
                        TagId = tag.Id,
                        TagData = tag
                    };
                    _dbContext.Add(narrativeTag);
                }
                else
                {
                    narrativeTag.NarrativeData = narrative;
                    narrativeTag.TagData = tag;
                }
            }




        }






    }
}