using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AutoMapper;
using Casperinc.MainSite.API.DTOModels;
using Casperinc.MainSite.API.Repositories;
using Casperinc.MainSite.API.Data.Models;
using CasperInc.MainSite.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.JsonPatch;

namespace Casperinc.MainSite.API.Controllers
{
    [Route("api/narratives")]
    public class NarrativeController : Controller
    {

        private INarrativeRepository _repo;
        private IUrlHelper _urlHelper;
        private ILogger<NarrativeController> _logger;

        public NarrativeController(INarrativeRepository repo, IUrlHelper urlHelper, ILogger<NarrativeController> logger)
        {
            _repo = repo;
            _urlHelper = urlHelper;
            _logger = logger;
        }


        [HttpGet(Name = "GetNarratives")]
        public IActionResult GetNarratives(NarrativeResourceParameters parms)
        {
            _logger.LogInformation($"Get Narratives with parms: Keyword Filter:{parms.KeywordFilter} | Page Number:{parms.PageNumber} | Page Size:{parms.PageSize}");

            var narrativesFromRepo = _repo.GetNarrativeList(parms);
            _logger.LogInformation($"IEnumerable<NarritveDataModel> Retrieved from Repo");
            _logger.LogTrace(JsonConvert.SerializeObject(narrativesFromRepo));

            var previousPageLink =
                narrativesFromRepo.HasPrevious ?
                               GetNarrativesResourceUri(parms, ResourceUriType.PreviousPage) :
                               null;

            _logger.LogInformation($"Previous Page Link: {previousPageLink}");

            var nextPageLink =
                narrativesFromRepo.HasNext ?
                               GetNarrativesResourceUri(parms, ResourceUriType.NextPage) :
                               null;

            _logger.LogInformation($"Next Page Link: {nextPageLink}");


            var paginationMetadata = new
            {
                totalCount = narrativesFromRepo.TotalCount,
                pageSize = narrativesFromRepo.PageSize,
                currentPage = narrativesFromRepo.CurrentPage,
                totalPages = narrativesFromRepo.TotalPages,
                previousPageLink = previousPageLink,
                nextPageLink = nextPageLink
            };
            _logger.LogInformation("Pagination Meta Data built");
            _logger.LogTrace(JsonConvert.SerializeObject(paginationMetadata));


            Response.Headers.Add("X-Pagination",
                                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));
            _logger.LogTrace("Pagination data added to Reponse Header");

            var returnNarratives =
                Mapper.Map<IEnumerable<NarrativeDTO>>(narrativesFromRepo);
            _logger.LogInformation($"IEnumerable<NarrativeDataModel> mapped to IEnumerable<NarrativeDTO>. ");
            _logger.LogTrace(JsonConvert.SerializeObject(returnNarratives));


            foreach (var narrative in returnNarratives)
            {
                if (narrative.Keywords == null) narrative.Keywords = new List<string>();

                narrative.Keywords.AddRange(
                    _repo.GetKeywordsForNarrative(narrative.Id)
                );
            }
            _logger.LogInformation("Tags Added to narrtives");
            _logger.LogTrace(JsonConvert.SerializeObject(returnNarratives));

            _logger.LogTrace("GetNarratives: Success");
            return Ok(returnNarratives);

        }

        [HttpGet("{narrativeId}", Name = "GetNarrative")]
        public IActionResult GetNarrative(Guid narrativeId)
        {
            _logger.LogInformation($"Get Narrative for {narrativeId}");

            var narrativeFromRepo = _repo.GetNarrative(narrativeId);
            if (narrativeFromRepo == null)
            {
                _logger.LogInformation($"Narraitve {narrativeId} Not found.");
                return NotFound();
            }
            else
            {
                _logger.LogInformation($"NarraitveDataModel {narrativeFromRepo.Id} retrieved.");
                _logger.LogTrace(JsonConvert.SerializeObject(narrativeFromRepo));
            }

            var returnNarrative = Mapper.Map<NarrativeDTO>(narrativeFromRepo);
            _logger.LogInformation($"NarrativeDataModel mapped to NarrativeDTO {returnNarrative.Id}. ");
            _logger.LogTrace(JsonConvert.SerializeObject(returnNarrative));

            if (returnNarrative.Keywords == null) returnNarrative.Keywords = new List<string>();
            returnNarrative.Keywords.AddRange(
                    _repo.GetKeywordsForNarrative(returnNarrative.Id)
                );

            _logger.LogInformation($"Keywords added to NarrativeDTO {returnNarrative.Id}. ");
            _logger.LogTrace(JsonConvert.SerializeObject(returnNarrative));

            _logger.LogTrace("GetNarrative: Success");
            return Ok(returnNarrative);

        }



        [HttpPost]
        public IActionResult CreateNarrative([FromBody] NarrativeToCreateDTO narrativeForCreate)
        {
            if (narrativeForCreate == null)
            {
                _logger.LogInformation("CreateNarratve: Failed to map input NarrativeToCreateDTO");
                _logger.LogTrace($"{ModelState.ErrorCount} | {ModelState.Values}");
                return BadRequest();
            }
            else
            {
                _logger.LogInformation("CreateNarratve: Failed to map input NarrativeToCreateDTO");
                _logger.LogTrace($"{ModelState.ErrorCount} | {ModelState.Values}");
            }

            if (narrativeForCreate.Keywords == null)
            {
                _logger.LogInformation("no valid key value");
                return BadRequest();
            }
            else if (narrativeForCreate.Keywords.Count == 0)
            {
                _logger.LogInformation("no valid key value");
                return BadRequest();
            }

            var newNarrative = Mapper.Map<NarrativeDataModel>(narrativeForCreate);

            if (newNarrative == null)
            {
                _logger.LogInformation("Mapping of NarrativeToCreaeDTO to NarrativeDataModel failed.");
                return BadRequest();
            }
            else
            {
                _logger.LogInformation("NarrativeToCreateDTO Mapped to Narrative DataModel");
                _logger.LogTrace(JsonConvert.SerializeObject(newNarrative));
            }
            

            foreach (var keyword in narrativeForCreate.Keywords)
            {
                _repo.CreateTag(keyword);
            }
            if(!_repo.SaveChanges())
            {
                _logger.LogInformation($"failed to add new narrative");
                throw new Exception($"Creating author failed on save");
            }

            var tags = _repo.GetTagsForKeywords(narrativeForCreate.Keywords);


            var addedNarrative = Mapper.Map<NarrativeDTO>(
                _repo.CreateNarrative(newNarrative, tags)
            );

            if(!_repo.SaveChanges())
            {
                _logger.LogInformation($"failed to add new narrative");
                throw new Exception($"Creating author failed on save");
            }

            if (addedNarrative == null)
            {
                _logger.LogInformation("Failure writing new narrative to database");
            }
            else
            {
                _logger.LogInformation("Narrative successfully written to database.");
                _logger.LogTrace(JsonConvert.SerializeObject(addedNarrative));
            }

            addedNarrative.Keywords = new List<string>();

            addedNarrative.Keywords.AddRange(
                    _repo.GetKeywordsForNarrative(addedNarrative.Id)
            );
            _logger.LogInformation("Keywords appended to NarrativeDTO");
            _logger.LogTrace(JsonConvert.SerializeObject(addedNarrative));

            return CreatedAtRoute(
                        "GetNarrative",
                        new { narrativeId = addedNarrative.Id },
                        addedNarrative);

            ;
        }



        [HttpPut("{narrativeId}")]
        public IActionResult fullUpdateForNarrativeById(Guid narrativeId, [FromBody] NarrativeToUpdateDTO narrativeForUpdate)
        {
            if(narrativeForUpdate == null) return BadRequest();

            if(!_repo.NarrativeExists(narrativeForUpdate.Id)) return NotFound();

            var narrativeFromRepo = _repo.GetNarrative(narrativeId);

            Mapper.Map(narrativeForUpdate, narrativeFromRepo);

            if (narrativeForUpdate.Keywords == null)
            {
                _logger.LogInformation("no valid key value");
                return BadRequest();
            }
            else if (narrativeForUpdate.Keywords.Count == 0)
            {
                _logger.LogInformation("no valid key value");
                return BadRequest();
            }

            foreach (var keyword in narrativeForUpdate.Keywords)
            {
                _repo.CreateTag(keyword);
            }

            if(!_repo.SaveChanges())
            {
                _logger.LogInformation($"failed to put narrative");
                throw new Exception($"Putting narrative failed on save");
            }
            var tags = _repo.GetTagsForKeywords(narrativeForUpdate.Keywords);

            _logger.LogInformation("Tags added to data model if needed.  All tag data reriteved for keywords.");
            _logger.LogTrace(JsonConvert.SerializeObject(tags));    

            _repo.UpdateNarrative(narrativeFromRepo, tags);

            if(!_repo.SaveChanges())
            {
                _logger.LogInformation($"failed to put narrative");
                throw new Exception($"Putting narrative failed on save");
            }

           return NoContent();

        }






        [HttpPatch("{narrativeId}")]
        public IActionResult partialUpdateForNarrativeById(Guid narrativeId, [FromBody] JsonPatchDocument<NarrativeToUpdateDTO> patchDoc)
        {
            // verify the request body was valid
            if (patchDoc == null) return BadRequest();

            // verify the requested narrative exist
            if (!_repo.NarrativeExists(narrativeId)) return NotFound();

            var narrativeFromRepo = _repo.GetNarrative(narrativeId);
            var keywordsFromRepo = _repo.GetKeywordsForNarrative(narrativeId);

            var narrativeToPatch = Mapper.Map<NarrativeToUpdateDTO>(narrativeFromRepo);
            narrativeToPatch.Keywords = new List<string>();
            narrativeToPatch.Keywords.AddRange(keywordsFromRepo);

            patchDoc.ApplyTo(narrativeToPatch, ModelState);

            TryValidateModel(narrativeToPatch);

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            if (narrativeToPatch.Keywords == null)
            {
                _logger.LogInformation("no valid key value");
                return BadRequest();
            }
            else if (narrativeToPatch.Keywords.Count == 0)
            {
                _logger.LogInformation("no valid key value");
                return BadRequest();
            }

            Mapper.Map(narrativeToPatch, narrativeFromRepo);

            
            foreach (var keyword in narrativeToPatch.Keywords)
            {
                _repo.CreateTag(keyword);
            }

            if(!_repo.SaveChanges())
            {
                _logger.LogInformation($"failed to patch narrative");
                throw new Exception($"Patching narrative failed on save");
            }

             var tags = _repo.GetTagsForKeywords(narrativeToPatch.Keywords);
            _logger.LogInformation("Tags added to data model if needed.  All tag data reriteved for keywords.");
            _logger.LogTrace(JsonConvert.SerializeObject(tags));

            _repo.UpdateNarrative(narrativeFromRepo, tags);

            if(!_repo.SaveChanges())
            {
                _logger.LogInformation($"failed to patch narrative");
                throw new Exception($"Patching narrative failed on save");
            }

            return NoContent();
        }




        [HttpDelete("{narrativeId}")]
        public IActionResult DeleteNarrativeWithID(Guid narrativeId)
        {
            if(!_repo.NarrativeExists(narrativeId)) return NotFound();

            var narrativeFromRepo = _repo.GetNarrative(narrativeId);

            if(narrativeFromRepo == null) return NotFound();

            
            _repo.DeleteNarrative(narrativeFromRepo);

            if(!_repo.SaveChanges())
            {
                _logger.LogInformation($"failed to delete narrative");
                throw new Exception($"Deleting narrative failed on save");
            }

            return NoContent();
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


