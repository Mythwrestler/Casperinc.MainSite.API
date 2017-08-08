using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AutoMapper;
using CasperInc.MainSite.API.DTOModels;
using CasperInc.MainSite.API.Repositories;
using CasperInc.MainSite.API.Data.Models;
using CasperInc.MainSite.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;

namespace CasperInc.MainSite.API.Controllers
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
