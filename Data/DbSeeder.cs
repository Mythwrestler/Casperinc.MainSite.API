using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Casperinc.MainSite.API.Data.Models;
using System.Collections.Generic;

namespace Casperinc.MainSite.API.Data
{

	public class DbSeeder
	{

		private MainSiteDbContext _dbContext;

		public DbSeeder(MainSiteDbContext dbContext)
		{
			_dbContext = dbContext;
		}


		public async Task ResetDBAsync()
		{
			_dbContext.NarrativeTagCrossWalk.RemoveRange(_dbContext.NarrativeTagCrossWalk);
			_dbContext.NarrativeData.RemoveRange(_dbContext.NarrativeData);
			_dbContext.TagData.RemoveRange(_dbContext.TagData);

			await _dbContext.SaveChangesAsync();

			await SeedAsync();
		}

		public async Task SeedAsync()
		{
			_dbContext.Database.EnsureCreated();

			if (await _dbContext.TagData.CountAsync() == 0)
			{
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
				UniqueId = 1,
				Id = new Guid("0c590e5b-882a-4b39-8576-3dd983f2759e"),
				KeyWord = "News"
				//CreatedDate = DateTime.Now,
				//UpdatedDate = DateTime.Now
			};

			// Create About Tag
			var aboutTag = new TagDataModel()
			{
				UniqueId = 2,
				Id = new Guid("9e6f1355-06c4-4ecb-b4a5-67d35d8e9af1"),
				KeyWord = "About"
				//CreatedDate = DateTime.Now,
				//UpdatedDate = DateTime.Now
			};

			_dbContext.TagData.AddRange(newsTag, aboutTag);
			await _dbContext.SaveChangesAsync();
		}

		private async Task CreateNarrativeData()
		{
			//Create Sample News Narratives
			List<NarrativeDataModel> newsItems = new List<NarrativeDataModel>();
			TagDataModel newsTag = await _dbContext.TagData.Where(t => t.KeyWord == "News").FirstOrDefaultAsync();

			newsItems.Add(new NarrativeDataModel()
			{
				UniqueId = 1,
				Id = new Guid("f6d78a09-fcc1-4a56-ab48-5ff9f2b81c10"),
				Title = "New Site!!! (1 of 2)",
				Description = "Welcome to the new Site!",
				BodyHtml =
					"<div>" +
						"New Site is a Site 1 " +
					"</div>"
				//CreatedOn = DateTime.Now,
				//UpdatedOn = DateTime.Now
			});
			newsItems.Add(new NarrativeDataModel()
			{
				UniqueId = 2,
				Id = new Guid("d12d4524-01b0-46a4-bb0b-a5c4ad522003"),
				Title = "New Site!!! (2 of 2)",
				Description = "Welcome to the new Site!",
				BodyHtml =
					"<div>" +
						"New Site is a Site 2 " +
					"</div>"
				//CreatedOn = DateTime.Now,
				//UpdatedOn = DateTime.Now
			});


			//Create Sample About Narratives
			List<NarrativeDataModel> abouts = new List<NarrativeDataModel>();
			TagDataModel aboutTag = await _dbContext.TagData.Where(t => t.KeyWord == "About").FirstOrDefaultAsync();

			abouts.Add(new NarrativeDataModel()
			{
				UniqueId = 3,
				Id = new Guid("c860838d-b9cc-49da-8924-283703318733"),
				Title = "About Me",
				Description = "A breif biography of Me.",
				BodyHtml =
					"<div>" +
					"Hi, my name is Mathew Casper and I'm a developer by trade.  " +
					"I learned my craft on an IBM iSeries.  " +
					"I have a B.S. with an advanced developer certificate from the Univeristy of Phoenix." +
					"</div>"
				//CreatedOn = DateTime.Now,
				//UpdatedOn = DateTime.Now
			});
			abouts.Add(new NarrativeDataModel()
			{ 
				UniqueId = 4,
				Id = new Guid("809d7a83-d583-4b1f-ae01-c2e17185f64c"),
				Title = "About The Site",
				Description = "A breif biography of the purpose of the site.",
				BodyHtml =
					"<div>" +
						"This site is my playground and my way to show off.  " +
						"It started with me wanting to learn .Net, but it has spiraled from there.  " +
					"</div>"
				//CreatedOn = DateTime.Now,
				//UpdatedOn = DateTime.Now
			});
			abouts.Add(new NarrativeDataModel()
			{
				UniqueId = 5,
				Id = new Guid("53c686c4-e865-4dde-83c7-ef7e969339e6"),
				Title = "Running on Linux",
				Description = "An explination of the operating environment.",
				BodyHtml =
					"<div>" +
						"The site is running on virtual linux server, running on real linux server.  " +
						"Part of this adventure has been learning Linux server administration. " +
					"</div>"
				//CreatedOn = DateTime.Now,
				//UpdatedOn = DateTime.Now
			});
			abouts.Add(new NarrativeDataModel()
			{
				UniqueId = 6,
				Id = new Guid("54ee3ce8-0f1e-4fee-b7e2-f1123bd09c2c"),
				Title = "Backed by .Net Core",
				Description = "A description of the backend services.",
				BodyHtml =
					"<div>" + "<a href=\"https://www.microsoft.com/net/core\" target=\"_blank\">" +
						".Net core </a> is a fantastic os agnostic version of the .Net platform.  Baked in " +
						"dependency injection, entity framework, testing, and of course .Net Core's cross-platform  " +
						"developement capabilites all make this platform for building web services.  ASP.Net Core " +
						"is used to backed everything you see on this site.  Some of the webservices were built " +
						"on macOS and some on Windows.  All of them are running on Linux. " +
					"</div>"
				//CreatedOn = DateTime.Now,
				//UpdatedOn = DateTime.Now
			});
			abouts.Add(new NarrativeDataModel()
			{
				UniqueId = 7,
				Id = new Guid("e5ea0de1-b2ab-49b0-be53-8a063a7b7790"),
				Title = "Angular 2",
				Description = "A modern user experience.",
				BodyHtml =
					"<div>" +
						"Angular 2 has proven to be a fantastic and easy to use front end architecture. " +
					"</div>"
				//CreatedOn = DateTime.Now,
				//UpdatedOn = DateTime.Now
			});


			// add the narrative items and the tag associations to the dbcontext
			foreach (NarrativeDataModel item in abouts)
			{
				var narrativeTag = new NarrativeTagDataModel()
				{
					NarrativeId = item.UniqueId,
					NarrativeData = item,
					TagId = aboutTag.UniqueId,
					TagData = aboutTag
				};
				_dbContext.Add(item);
				_dbContext.Add(narrativeTag);
			}
			foreach (NarrativeDataModel item in newsItems)
			{
				var narrativeTag = new NarrativeTagDataModel()
				{
					NarrativeId = item.UniqueId,
					NarrativeData = item,
					TagId = newsTag.UniqueId,
					TagData = newsTag
				};
				_dbContext.Add(item);
				_dbContext.Add(narrativeTag);
			}
			await _dbContext.SaveChangesAsync();
		}

	}

}