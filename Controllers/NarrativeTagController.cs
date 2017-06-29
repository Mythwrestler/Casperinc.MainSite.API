using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AutoMapper;
using Casperinc.MainSite.API.DTOModels;
using Casperinc.MainSite.API.Repositories;
using Casperinc.MainSite.API.Data.Models;

namespace Casperinc.MainSite.API.Controllers
{
    [Route("api/narratives/{narrativeId}/tags")]
    public class NarrativeTagController : Controller
    {

        private INarrativeRepository _repo;

        public NarrativeTagController(INarrativeRepository repo)
        {
            _repo = repo;
        }


        [HttpGet]
        public IActionResult GetTagsForNarrative(Guid narrativeId)
        {
            if(!_repo.NarrativeExists(narrativeId)) return BadRequest();

            var tagsFromRepo = Mapper.Map<IEnumerable<TagDTO>>(_repo.getTagsForNarrative(narrativeId));

            return Ok(tagsFromRepo);
        }

        [HttpGet("{keyword}", Name = "GetTagByKeyword")]
        public IActionResult GetTag(Guid narrativeId, string keyword)
        {
            if(!_repo.NarrativeExists(narrativeId)) return BadRequest();
            if(!_repo.TagExists(keyword)) return BadRequest();

            var tagFromRepo = Mapper.Map<TagDTO>(_repo.GetTag(keyword));

            return Ok(tagFromRepo);
        }

        // [HttpPost]
        // public IActionResult CreateNarrative ([FromBody] TagToCreateDTO tag)
        // {
        //     if(narrative == null) return BadRequest();
        //     if(narrative.Keywords == null) return BadRequest();

        //     var newNarrative = Mapper.Map<NarrativeDataModel>(narrative);
        //     var tags = new List<TagDataModel>();
        //     foreach (var keyword in narrative.Keywords)
        //     {
        //         tags.Add(_repo.CreateTag(keyword));
        //     }
            
        //     var addedNarrative = Mapper.Map<NarrativeDTO>(
        //         _repo.CreateNarrative(newNarrative, tags)
        //     );

        //     addedNarrative.Keywords = new List<string>();
        //     addedNarrative.Keywords.AddRange(
        //             _repo.GetKeywordsForNarrative(addedNarrative.Id)
        //     );


        //     return CreatedAtRoute(
        //                 "GetNarrative",
        //                 new {narrativeId = addedNarrative.Id},
        //                 addedNarrative);
            
        //     ;
        //     return Ok();

        // }


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