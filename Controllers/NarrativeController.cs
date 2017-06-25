using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AutoMapper;
using MainSiteCore.DTOModels;
using CasperInc.MainSiteCore.Repositories;

namespace CasperInc.MainSiteCore.Controllers
{
    [Route("api/narratives")]
    public class NarrativeController : Controller
    {

        private INarrativeRepository _repo;

        public NarrativeController(INarrativeRepository repo)
        {
            _repo = repo;
        }


        [HttpGet]
        public IActionResult GetNarratives()
        {
            var narrativesFromRepo = _repo.GetNarrativeList();

            var returnNarratives =
                Mapper.Map<IEnumerable<NarrativeDTO>>(narrativesFromRepo);

            foreach (var narrative in returnNarratives)
            {
                if (narrative.Tags == null) narrative.Tags = new List<TagDTO>();

                narrative.Tags.AddRange(
                    Mapper.Map<IEnumerable<TagDTO>>(_repo.getTagsForNarrative(narrative.Id))
                );
            }

            return Ok(returnNarratives);

        }

        [HttpGet("{keyword}")]
		public IActionResult GetNarrativesWithKeyword(string keyword)
		{

            if (!_repo.TagExists(keyword)) return NotFound();

            var narrativesFromRepo = _repo.GetNarrativeListWithKeyword(keyword);

			var returnNarratives =
				Mapper.Map<IEnumerable<NarrativeDTO>>(narrativesFromRepo);

			foreach (var narrative in returnNarratives)
			{
				if (narrative.Tags == null) narrative.Tags = new List<TagDTO>();

				narrative.Tags.AddRange(
					Mapper.Map<IEnumerable<TagDTO>>(_repo.getTagsForNarrative(narrative.Id))
				);
			}

			return Ok(returnNarratives);

		}

        [HttpGet("{narrativeId}")]
        public IActionResult GetNarrative(Guid narrativeId)
        {
            var narrativeFromRepo = _repo.GetNarrative(narrativeId);
            if (narrativeFromRepo == null) return NotFound();

            var returnNarrative = Mapper.Map<NarrativeDTO>(narrativeFromRepo);

            if (returnNarrative.Tags == null) returnNarrative.Tags = new List<TagDTO>();
			returnNarrative.Tags.AddRange(
					Mapper.Map<IEnumerable<TagDTO>>(_repo.getTagsForNarrative(returnNarrative.Id))
				);

            return Ok(returnNarrative);

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

    }
}