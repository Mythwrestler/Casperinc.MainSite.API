using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


using CasperInc.MainSiteCore.ViewModels;
using AutoMapper;
using CasperInc.MainSiteCore.Data;
using CasperInc.MainSiteCore.Data.Models;

namespace CasperInc.MainSiteCore.Controllers
{
    [Route("api/MainSiteCore/[controller]")]
    public class NarrativeController : Controller
    {

        private MainSiteCoreDBContext _dbContext;
        private IMapper _mapper;

        public NarrativeController(MainSiteCoreDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGetAttribute]
        public IActionResult GetNarratives()
        {
            var narratives = _dbContext.NarrativeData.ToArray();

            return new JsonResult(toNarrativeList(narratives), DefaultJsonSettings);
        }

        [HttpGetAttribute("GetAboutNarratives")]
        public IActionResult GetAboutNarratives()
        {
            var narratives = _dbContext.NarrativeData
                                            .Where(n => n.NarrativeTags
                                                .All(t => t.TagKeyWord == "About")
                                            ).ToArray();

            return new JsonResult(toNarrativeList(narratives), DefaultJsonSettings);
        }



        // Mappers

        private List<Narrative> toNarrativeList (IEnumerable<NarrativeDataModel> narrativeListing) 
        {
            var outList = new List<Narrative>();
            foreach (var narrativeData in narrativeListing)
            {
                outList.Add(_mapper.Map<Narrative>(narrativeData));
            }
            return outList;

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