using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AutoMapper;
using Casperinc.MainSite.API.DTOModels;
using Casperinc.MainSite.API.Repositories;
using Casperinc.MainSite.API.Data.Models;
using Casperinc.MainSite.API.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;

namespace Casperinc.MainSite.API.Controllers
{
    [Route("mainsite/api/narratives/{narrativeId}/comments")]
    public class CommentController : Controller
    {
        private ICommentRepository _commentRepo;

        CommentController(ICommentRepository commentRepo)
        {
            _commentRepo = commentRepo;
        }

    }

}
