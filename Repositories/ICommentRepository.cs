using System;
using System.Collections.Generic;
using Casperinc.MainSite.API.Data.Models;
using Casperinc.MainSite.Helpers;

namespace Casperinc.MainSite.API.Repositories
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