using System;
using System.Collections.Generic;
using CasperInc.MainSite.API.Data.Models;
using CasperInc.MainSite.Helpers;

namespace CasperInc.MainSite.API.Repositories
{
    public interface ICommentRepository
    {

        bool CommentExists(Guid commentID);

        CommentDataModel GetComment (Guid commentID);

        PagedList<CommentDataModel> GetCommentsForNarrative (Guid narrativeId, CommentResourceParameters parms);

        bool addComment (CommentDataModel comment);

        bool SaveChanges();

    }

}