using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CasperInc.MainSite.API.Data.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using CasperInc.MainSite.Helpers;

namespace CasperInc.MainSite.API.Data
{

    public class DbSeeder
    {

        private MainSiteDbContext _dbContext;
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<UserDataModel> _userManager;

        public DbSeeder(MainSiteDbContext dbContext,
            RoleManager<IdentityRole> roleManager,
            UserManager<UserDataModel> userManager
        )
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }


        public async Task ResetDBAsync()
        {
            _dbContext.NarrativeTagCrossWalk.RemoveRange(_dbContext.NarrativeTagCrossWalk);
            _dbContext.NarrativeData.RemoveRange(_dbContext.NarrativeData);
            _dbContext.TagData.RemoveRange(_dbContext.TagData);
            _dbContext.UserClaims.RemoveRange(_dbContext.UserClaims);
            _dbContext.UserLogins.RemoveRange(_dbContext.UserLogins);
            _dbContext.UserRoles.RemoveRange(_dbContext.UserRoles);
            _dbContext.UserTokens.RemoveRange(_dbContext.UserTokens);
            _dbContext.Users.RemoveRange(_dbContext.Users);
            _dbContext.RoleClaims.RemoveRange(_dbContext.RoleClaims);
            _dbContext.Roles.RemoveRange(_dbContext.Roles);


            await _dbContext.SaveChangesAsync();

            await SeedAsync();
        }

        public async Task SeedAsync()
        {
            _dbContext.Database.EnsureCreated();


            if (await _dbContext.Users.CountAsync() == 0)
            {
                await CreateUserAsync();
            }

            if (await _dbContext.TagData.CountAsync() == 0)
            {
                await CreateTagData();
            }

            if (await _dbContext.NarrativeData.CountAsync() == 0)
            {
                await CreateNarrativeData();
            }


        }


        private async Task CreateUserAsync()
        {

            string role_Administrators = "Administrators";
            string role_Registered = "Registered";

            // Create Roles
            if (!await _roleManager.RoleExistsAsync(role_Administrators))
            {
                await _roleManager.CreateAsync(new IdentityRole(role_Administrators));
                await _dbContext.SaveChangesAsync();
            }
            if (!await _roleManager.RoleExistsAsync(role_Registered))
            {
                await _roleManager.CreateAsync(new IdentityRole(role_Registered));
                await _dbContext.SaveChangesAsync();
            }



            // Create the "Admin" ApplicationUser account (if it does not exist)
            var user_admin = new UserDataModel()
            {
                UserName = "Admin",
                Email = "admin@casperinc.expert"
            };


            // Insert "Admin" into the Database and assign "Administrator" Role
            if (await _userManager.FindByIdAsync(user_admin.Id) == null)
            {
                await _userManager.CreateAsync(user_admin, "Pass4Admin");
                await _userManager.AddToRoleAsync(user_admin, role_Administrators);
                //Mark Email as confirmed and remove Account Lock
                user_admin.EmailConfirmed = true;
                user_admin.LockoutEnabled = false;
            }
            await _dbContext.SaveChangesAsync();

            var user_casperm = new UserDataModel()
            {
                UserName = "casperm",
                Email = "mythwrestler@casperinc.expert"
            };
            if (await _userManager.FindByIdAsync(user_casperm.Id) == null)
            {
                await _userManager.CreateAsync(user_casperm, "tempPass1sLegit!");
                await _userManager.AddToRoleAsync(user_casperm, role_Registered);
                //Mark Email as confirmed and remove Account Lock
                user_casperm.EmailConfirmed = true;
                user_casperm.LockoutEnabled = false;
            }
            await _dbContext.SaveChangesAsync();


            var user_caspermAdmin = new UserDataModel()
            {
                UserName = "casperm-admin",
                Email = "mathew.casper@casperinc.expert"
            };
            if (await _userManager.FindByIdAsync(user_caspermAdmin.Id) == null)
            {
                await _userManager.CreateAsync(user_caspermAdmin, "tempPass1sLegit!");
                await _userManager.AddToRoleAsync(user_caspermAdmin, role_Administrators);
                //Mark Email as confirmed and remove Account Lock
                user_caspermAdmin.EmailConfirmed = true;
                user_caspermAdmin.LockoutEnabled = false;
            }
            await _dbContext.SaveChangesAsync();
        }


        private async Task CreateTagData()
        {
            // Create News Tag
            var newsTag = new TagDataModel()
            {
                UniqueId = 1,
                GuidId = new Guid("0c590e5b-882a-4b39-8576-3dd983f2759e"),
                KeyWord = "News"
            };

            // Create About Tag
            var aboutTag = new TagDataModel()
            {
                UniqueId = 2,
                GuidId = new Guid("9e6f1355-06c4-4ecb-b4a5-67d35d8e9af1"),
                KeyWord = "About"
            };

            _dbContext.TagData.AddRange(newsTag, aboutTag);
            await _dbContext.SaveChangesAsync();
        }

        private async Task CreateNarrativeData()
        {
            UserDataModel user = await _dbContext.Users.Where(u => u.UserName == "casperm").FirstOrDefaultAsync();

            //Create Sample News Narratives
            List<NarrativeDataModel> newsItems = new List<NarrativeDataModel>();
            TagDataModel newsTag = await _dbContext.TagData.Where(t => t.KeyWord == "News").FirstOrDefaultAsync();

            newsItems.Add(new NarrativeDataModel()
            {
                UniqueId = 1,
                GuidId = new Guid("f6d78a09-fcc1-4a56-ab48-5ff9f2b81c10"),
                UserId = user.Id,
                Title = "New Site!!! (1 of 2)",
                Type = NarrativeTypes.News,
                Description = "Welcome to the new Site!",
                BodyHtml =
                    "<div>" +
                        "New Site is a Site 1 " +
                    "</div>"
            });
            newsItems.Add(new NarrativeDataModel()
            {
                UniqueId = 2,
                GuidId = new Guid("d12d4524-01b0-46a4-bb0b-a5c4ad522003"),
                UserId = user.Id,
                Title = "New Site!!! (2 of 2)",
                Type = NarrativeTypes.News,
                Description = "Welcome to the new Site!",
                BodyHtml =
                    "<div>" +
                        "New Site is a Site 2 " +
                    "</div>"
            });


            //Create Sample About Narratives
            List<NarrativeDataModel> abouts = new List<NarrativeDataModel>();
            TagDataModel aboutTag = await _dbContext.TagData.Where(t => t.KeyWord == "About").FirstOrDefaultAsync();

            abouts.Add(new NarrativeDataModel()
            {
                UniqueId = 3,
                GuidId = new Guid("c860838d-b9cc-49da-8924-283703318733"),
                UserId = user.Id,
                Title = "About Me",
                Type = NarrativeTypes.About,
                Description = "A breif biography of Me.",
                BodyHtml =
                    "<div>" +
                    "Hi, my name is Mathew Casper and I'm a developer by trade.  " +
                    "I learned my craft on an IBM iSeries.  " +
                    "I have a B.S. with an advanced developer certificate from the Univeristy of Phoenix." +
                    "</div>"
            });
            abouts.Add(new NarrativeDataModel()
            {
                UniqueId = 4,
                GuidId = new Guid("809d7a83-d583-4b1f-ae01-c2e17185f64c"),
                UserId = user.Id,
                Title = "About The Site",
                Type = NarrativeTypes.About,
                Description = "A breif biography of the purpose of the site.",
                BodyHtml =
                    "<div>" +
                        "This site is my playground and my way to show off.  " +
                        "It started with me wanting to learn .Net, but it has spiraled from there.  " +
                    "</div>"
            });
            abouts.Add(new NarrativeDataModel()
            {
                UniqueId = 5,
                GuidId = new Guid("53c686c4-e865-4dde-83c7-ef7e969339e6"),
                UserId = user.Id,
                Title = "Running on Linux",
                Type = NarrativeTypes.About,
                Description = "An explination of the operating environment.",
                BodyHtml =
                    "<div>" +
                        "The site is running on virtual linux server, running on real linux server.  " +
                        "Part of this adventure has been learning Linux server administration. " +
                    "</div>"
            });
            abouts.Add(new NarrativeDataModel()
            {
                UniqueId = 6,
                GuidId = new Guid("54ee3ce8-0f1e-4fee-b7e2-f1123bd09c2c"),
                UserId = user.Id,
                Title = "Backed by .Net Core",
                Type = NarrativeTypes.About,
                Description = "A description of the backend services.",
                BodyHtml =
                    "<div>" + "<a href=\"https://www.microsoft.com/net/core\" target=\"_blank\">" +
                        ".Net core </a> is a fantastic os agnostic version of the .Net platform.  Baked in " +
                        "dependency injection, entity framework, testing, and of course .Net Core's cross-platform  " +
                        "developement capabilites all make this platform for building web services.  ASP.Net Core " +
                        "is used to backed everything you see on this site.  Some of the webservices were built " +
                        "on macOS and some on Windows.  All of them are running on Linux. " +
                    "</div>"
            });
            abouts.Add(new NarrativeDataModel()
            {
                UniqueId = 7,
                GuidId = new Guid("e5ea0de1-b2ab-49b0-be53-8a063a7b7790"),
                UserId = user.Id,
                Title = "Angular 2",
                Type = NarrativeTypes.About,
                Description = "A modern user experience.",
                BodyHtml =
                    "<div>" +
                        "Angular 2 has proven to be a fantastic and easy to use front end architecture. " +
                    "</div>"
            });


            // add the narrative items and the tag associations to the dbcontext
            foreach (NarrativeDataModel narraitve in abouts)
            {
                var narrativeTag = new NarrativeTagDataModel()
                {
                    NarrativeId = narraitve.UniqueId,
                    NarrativeData = narraitve,
                    TagId = aboutTag.UniqueId,
                    TagData = aboutTag
                };
                _dbContext.Add(narraitve);
                _dbContext.Add(narrativeTag);


            }
            foreach (NarrativeDataModel narraitve in newsItems)
            {
                var narrativeTag = new NarrativeTagDataModel()
                {
                    NarrativeId = narraitve.UniqueId,
                    NarrativeData = narraitve,
                    TagId = newsTag.UniqueId,
                    TagData = newsTag
                };
                _dbContext.Add(narraitve);
                _dbContext.Add(narrativeTag);
            }
            await _dbContext.SaveChangesAsync();
        }

    }

}