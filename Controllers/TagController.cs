using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AutoMapper;
using Casperinc.MainSite.API.DTOModels;
using Casperinc.MainSite.API.Repositories;
using Casperinc.MainSite.API.Data.Models;
using System.Linq;

namespace Casperinc.MainSite.API.Controllers
{
    [Route("api/tags")]
    public class TagController : Controller
    {

        private INarrativeRepository _repo;

        public TagController(INarrativeRepository repo)
        {
            _repo = repo;
        }


        [HttpGet]
        public IActionResult GetTags()
        {
            var tagsFromRepo = _repo.GetTags();

            var returnTags =
                Mapper.Map<IEnumerable<TagDTO>>(tagsFromRepo);

            foreach (var tag in returnTags)
            {
                tag.Narratives = _repo.GetNarrativeListWithKeyword(tag.KeyWord)
                    .Select(t => t.Id)
                    .AsEnumerable();
            }

            return Ok(returnTags);

        }

        //[HttpGet("{narrativeId}", Name = "GetNarrative")]
        //public IActionResult GetNarrative(Guid narrativeId)
        //{
        //    var narrativeFromRepo = _repo.GetNarrative(narrativeId);
        //    if (narrativeFromRepo == null) return NotFound();

        //    var returnNarrative = Mapper.Map<NarrativeDTO>(narrativeFromRepo);

        //    if (returnNarrative.Keywords == null) returnNarrative.Keywords = new List<string>();
        //    returnNarrative.Keywords.AddRange(
        //            _repo.GetKeywordsForNarrative(returnNarrative.Id)
        //        );

        //    return Ok(returnNarrative);

        //}

        //[HttpPost]
        //public IActionResult CreateNarrative ([FromBody] NarrativeToCreateDTO narrative)
        //{
        //    if(narrative == null) return BadRequest();
        //    if(narrative.Keywords == null) return BadRequest();

        //    var newNarrative = Mapper.Map<NarrativeDataModel>(narrative);
        //    var tags = new List<TagDataModel>();
        //    foreach (var keyword in narrative.Keywords)
        //    {
        //        tags.Add(_repo.CreateTag(keyword));
        //    }
            
        //    var addedNarrative = Mapper.Map<NarrativeDTO>(
        //        _repo.CreateNarrative(newNarrative, tags)
        //    );

        //    addedNarrative.Keywords = new List<string>();
        //    addedNarrative.Keywords.AddRange(
        //            _repo.GetKeywordsForNarrative(addedNarrative.Id)
        //    );


        //    return CreatedAtRoute(
        //                "GetNarrative",
        //                new {narrativeId = addedNarrative.Id},
        //                addedNarrative);
            
        //    ;
        //}


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

    }
}