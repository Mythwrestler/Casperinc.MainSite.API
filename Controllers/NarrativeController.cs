using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AutoMapper;
using Casperinc.MainSite.API.DTOModels;
using Casperinc.MainSite.API.Repositories;
using Casperinc.MainSite.API.Data.Models;
using CasperInc.MainSite.Helpers;

namespace Casperinc.MainSite.API.Controllers
{
    [Route("api/narratives")]
    public class NarrativeController : Controller
    {

        private INarrativeRepository _repo;
        private IUrlHelper _urlHelper;

        public NarrativeController(INarrativeRepository repo, IUrlHelper urlHelper)
        {
            _repo = repo;
            _urlHelper = urlHelper;
        }


        [HttpGet(Name = "GetNarratives")]
        public IActionResult GetNarratives(NarrativeResourceParameters parms)
        {
            var narrativesFromRepo = _repo.GetNarrativeList(parms);

 
            var previousPageLink = 
                narrativesFromRepo.HasPrevious? 
                               GetNarrativesResourceUri(parms,ResourceUriType.PreviousPage) :
                               null;

			var nextPageLink =
				narrativesFromRepo.HasNext ?
							   GetNarrativesResourceUri(parms, ResourceUriType.NextPage) :
							   null;

            var paginationMetadata = new
            {
                totalCount = narrativesFromRepo.TotalCount,
                pageSize = narrativesFromRepo.PageSize,
                currentPage = narrativesFromRepo.CurrentPage,
                totalPages = narrativesFromRepo.TotalPages,
                previousPageLink = previousPageLink,
                nextPageLink = nextPageLink
            };
           

            Response.Headers.Add("X-Pagination",
                                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            var returnNarratives =
                Mapper.Map<IEnumerable<NarrativeDTO>>(narrativesFromRepo);

            foreach (var narrative in returnNarratives)
            {
                if (narrative.Keywords == null) narrative.Keywords = new List<string>();

                narrative.Keywords.AddRange(
                    _repo.GetKeywordsForNarrative(narrative.Id)
                );
            }
            
            return Ok(returnNarratives);

        }

        [HttpGet("{narrativeId}", Name = "GetNarrative")]
        public IActionResult GetNarrative(Guid narrativeId)
        {
            var narrativeFromRepo = _repo.GetNarrative(narrativeId);
            if (narrativeFromRepo == null) return NotFound();

            var returnNarrative = Mapper.Map<NarrativeDTO>(narrativeFromRepo);

            if (returnNarrative.Keywords == null) returnNarrative.Keywords = new List<string>();
            returnNarrative.Keywords.AddRange(
                    _repo.GetKeywordsForNarrative(returnNarrative.Id)
                );

            return Ok(returnNarrative);

        }

        [HttpPost]
        public IActionResult CreateNarrative ([FromBody] NarrativeToCreateDTO narrative)
        {
            if(narrative == null) return BadRequest();
            if(narrative.Keywords == null) return BadRequest();

            var newNarrative = Mapper.Map<NarrativeDataModel>(narrative);
            var tags = new List<TagDataModel>();
            foreach (var keyword in narrative.Keywords)
            {
                tags.Add(_repo.CreateTag(keyword));
            }
            
            var addedNarrative = Mapper.Map<NarrativeDTO>(
                _repo.CreateNarrative(newNarrative, tags)
            );

            addedNarrative.Keywords = new List<string>();
            addedNarrative.Keywords.AddRange(
                    _repo.GetKeywordsForNarrative(addedNarrative.Id)
            );


            return CreatedAtRoute(
                        "GetNarrative",
                        new {narrativeId = addedNarrative.Id},
                        addedNarrative);
            
            ;
        }


        private JsonSerializerSettings DefaultJsonSettings
        {
            get
            {
                return new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented
                };
            }
        }



        private string GetNarrativesResourceUri(
            NarrativeResourceParameters narrativeResourceParameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetNarratives",
                       new
                       {
                           keywordFilter = narrativeResourceParameters.KeywordFilter,
                           pageNumber = narrativeResourceParameters.PageNumber - 1,
                           pageSize = narrativeResourceParameters.PageSize
                       });
				case ResourceUriType.NextPage:
					return _urlHelper.Link("GetNarratives",
					   new
					   {
						   keywordFilter = narrativeResourceParameters.KeywordFilter,
						   pageNumber = narrativeResourceParameters.PageNumber + 1,
						   pageSize = narrativeResourceParameters.PageSize
					   });
                default:
					return _urlHelper.Link("GetNarratives",
					   new
					   {
						   keywordFilter = narrativeResourceParameters.KeywordFilter,
						   pageNumber = narrativeResourceParameters.PageNumber,
						   pageSize = narrativeResourceParameters.PageSize
					   });
                    
            }
                                

        }



    }
}


