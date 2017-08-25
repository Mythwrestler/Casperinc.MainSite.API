using System;
using System.Collections.Generic;
using System.Linq;
using Casperinc.MainSite.API.Data;
using Casperinc.MainSite.API.Data.Models;
using Casperinc.MainSite.API.Helpers;

namespace Casperinc.MainSite.API.Repositories
{
    public class CommentRepository : ICommentRepository
    {

        private MainSiteDbContext _dbContext;

        public CommentRepository(MainSiteDbContext dbcontext, INarrativeRepository narrativeRepo)
        {
            _dbContext = dbcontext;
        }

        public bool CommentExists(Guid commentID)
        {
            return _dbContext.CommentData.Any(c => c.GuidId == commentID);
        }

        public CommentDataModel GetComment (Guid commentID)
        {
            var comment = _dbContext.CommentData.Where(c => c.GuidId == commentID).FirstOrDefault();
            return comment;
        }

        public PagedList<CommentDataModel> GetCommentsForNarrative (Guid narrativeId, CommentResourceParameters parms)
        {
            var commentsBeforePaging = _dbContext.CommentData.Where(c => c.Narrative.GuidId == narrativeId).AsQueryable();

            return  PagedList<CommentDataModel>.Create(
                source: commentsBeforePaging,
                pageNumber: parms.PageNumber,
                pageSize: parms.PageSize
            );
        }

        public bool addComment (CommentDataModel comment)
        {
            comment.GuidId = Guid.NewGuid();
            _dbContext.CommentData.Add(comment);
            return SaveChanges();
        }


        public bool SaveChanges()
        {
            return(_dbContext.SaveChanges() >= 0);
        }



    }

}