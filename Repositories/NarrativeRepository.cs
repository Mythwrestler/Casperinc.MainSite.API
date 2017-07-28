using System;
using System.Collections.Generic;
using System.Linq;
using CasperInc.MainSite.API.Data;
using CasperInc.MainSite.API.Data.Models;
using CasperInc.MainSite.Helpers;

namespace CasperInc.MainSite.API.Repositories
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
            return _dbContext.NarrativeData.Any(n => n.GuidId == narrativeId);
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
            return _dbContext.NarrativeData.Where(n => n.GuidId == narrativeId).FirstOrDefault();
        }



        public IEnumerable<TagDataModel> GetTagsForNarrative(Guid narrativeId)
        {
            var tags = _dbContext.NarrativeTagCrossWalk
                                 .Where(n => n.NarrativeData.GuidId == narrativeId)
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
            return _dbContext.TagData.Select(t => t.GuidId).Contains(tagId);
        }



        public TagDataModel CreateTag(string keyword)
        {
            if (!TagExists(keyword))
            {

                _dbContext.TagData.Add(
                     new TagDataModel()
                     {
                         GuidId = Guid.NewGuid(),
                         KeyWord = keyword
                     }
                );

            }

            return _dbContext.TagData.Where(t => t.KeyWord == keyword).FirstOrDefault();
        }



        public TagDataModel GetTag(Guid tagId)
        {
            return _dbContext.TagData.Where(t => t.GuidId == tagId).FirstOrDefault();
        }



        public TagDataModel GetTag(string keyword)
        {
            return _dbContext.TagData.Where(t => t.KeyWord == keyword).FirstOrDefault();
        }




        public NarrativeDataModel CreateNarrative(NarrativeDataModel narrative, IEnumerable<TagDataModel> tags)
        {
            narrative.GuidId = Guid.NewGuid();
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
            .Where(nt => nt.NarrativeData.GuidId == narrative.GuidId)
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
            .Where(nt => nt.NarrativeData.GuidId == narrative.GuidId && tags.Select(t => t.GuidId).Contains(nt.TagData.GuidId) == false )
            .ToList();


            if(narrativeTagsToRemove.Count != 0) {
                _dbContext.NarrativeTagCrossWalk.RemoveRange(narrativeTagsToRemove);
            }


            // Add new & update existing crosswalks
            foreach (var tag in tags)
            {
                // attempt to get existing narrativeTag crosswalk entry
                var narrativeTag = _dbContext.NarrativeTagCrossWalk
                    .Where(nt => nt.NarrativeData.GuidId == narrative.GuidId && nt.TagData.GuidId == tag.GuidId)
                    .FirstOrDefault();

                // if there is not one, remove it.
                if (narrativeTag == null)
                {
                    narrativeTag = new NarrativeTagDataModel()
                    {
                        NarrativeId = narrative.UniqueId,
                        NarrativeData = narrative,
                        TagId = tag.UniqueId,
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