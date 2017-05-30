using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CasperInc.MainSiteCore.Data.Models;
using System.Collections.Generic;

namespace CasperInc.MainSiteCore.Data {

    public class DbSeeder {

        private MainSiteCoreDBContext _dbContext;

        public DbSeeder (MainSiteCoreDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SeedAsync() 
        {
            _dbContext.Database.EnsureCreated();

            if (await _dbContext.TagData.CountAsync() == 0) {
                 await CreateTagData();
            }

            if (await _dbContext.NarrativeData.CountAsync() == 0)
            {
                await CreateNarrativeData();
            }

        }


        private async Task CreateTagData()
        {
            // Create News Tag
            var newsTag = new TagDataModel()
            {
                KeyWord="News",
                CreatedDate= DateTime.Now,
                UpdatedDate = DateTime.Now
            };

            // Create About Tag
            var aboutTag = new TagDataModel()
            {
                KeyWord="About",
                CreatedDate= DateTime.Now,
                UpdatedDate = DateTime.Now
            };

            _dbContext.TagData.AddRange(newsTag, aboutTag);
            await _dbContext.SaveChangesAsync();
        }

        private async Task CreateNarrativeData()
        {
            //Create Sample News Narratives
            List<NarrativeDataModel> newsItems = new List<NarrativeDataModel>();
            TagDataModel newsTag = await _dbContext.TagData.Where(t => t.KeyWord=="News").FirstOrDefaultAsync();

            newsItems.Add(new NarrativeDataModel() 
            {
                Title="New Site!!! (1 of 2)",
                Description="Welcome to the new Site!",
                BodyHtml=
                    "<div>" +
                        "New Site is a Site 1 " +
                    "</div>",
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            });
            newsItems.Add(new NarrativeDataModel() 
            {
                Title="New Site!!! (2 of 2)",
                Description="Welcome to the new Site!",
                BodyHtml=
                    "<div>" +
                        "New Site is a Site 2 " +
                    "</div>",
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            });
            

            //Create Sample About Narratives
            List<NarrativeDataModel> abouts = new List<NarrativeDataModel>();
            TagDataModel aboutTag = await _dbContext.TagData.Where(t => t.KeyWord=="About").FirstOrDefaultAsync();

            abouts.Add(new NarrativeDataModel() 
            {
                Title="About Me",
                Description="A breif biography of Me.",
                BodyHtml=
                    "<div>" +
                    "Hi, my name is Mathew Casper and I'm a developer by trade.  " +
                    "I learned my craft on an IBM iSeries.  " +
                    "I have a B.S. with an advanced developer certificate from the Univeristy of Phoenix." +
                    "</div>",
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            });
            abouts.Add(new NarrativeDataModel() 
            {
                Title="About The Site",
                Description="A breif biography of the purpose of the site.",
                BodyHtml=
                    "<div>" +
                        "This site is my playground and my way to show off.  " +
                        "It started with me wanting to learn .Net, but it has spiraled from there.  " +
                    "</div>",
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            });
            abouts.Add(new NarrativeDataModel() 
            {
                Title="Running on Linux",
                Description="An explination of the operating environment.",
                BodyHtml=
                    "<div>" +
                        "The site is running on virtual linux server, running on real linux server.  " +
                        "Part of this adventure has been learning Linux server administration. " +
                    "</div>",
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            });
            abouts.Add(new NarrativeDataModel() 
            {
                Title="Backed by .Net Core",
                Description="A description of the backend services.",
                BodyHtml=
                    "<div>" +
                        ".Net core is a fantastic os agnostic version of the .Net platform.  Baked in " +
                        "dependency injection, entity framework, testing, and of course .Net Core's cross-platform  " +
                        "developement capabilites all make this platform for building web services.  ASP.Net Core " +
                        "is used to backed everything you see on this site.  Some of the webservices were built " +
                        "on macOS and some on Windows.  All of them are running on Linux. " +
                    "</div>",
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            });
            abouts.Add(new NarrativeDataModel() 
            {
                Title="About Me",
                Description="A breif biography of Me.",
                BodyHtml=
                    "<div>" +
                        "Angular 2 has proven to be a fantastic and easy to use front end architecture. " +
                    "</div>",
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            });


            // add the narrative items and the tag associations to the dbcontext
            foreach(NarrativeDataModel item in abouts){
                 var narrativeTag = new NarrativeTagDataModel()
                    {
                        NarrativeId = item.Id,
                        NarrativeData = item,
                        TagId = aboutTag.Id,
                        TagData = aboutTag
                    };
                 _dbContext.Add(item);
                 _dbContext.Add(narrativeTag);
            }
            foreach(NarrativeDataModel item in newsItems){
                 var narrativeTag = new NarrativeTagDataModel()
                    {
                        NarrativeId = item.Id,
                        NarrativeData = item,
                        TagId = newsTag.Id,
                        TagData = newsTag
                    };
                 _dbContext.Add(item);
                 _dbContext.Add(narrativeTag);
            }
            await _dbContext.SaveChangesAsync();
        }

    }

}