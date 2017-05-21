using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CasperInc.MainSiteCore.Data;
using CasperInc.MainSiteCore.Data.Models;
using CasperInc.MainSiteCore.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CasperInc.MainSiteCore
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services.AddEntityFrameworkSqlite();

            services.AddDbContext<MainSiteCoreDBContext>();
            
            services.AddSingleton<DbSeeder>();

            var MapConfig = new AutoMapper.MapperConfiguration(config =>
                {
                    config.CreateMap<NarrativeDataModel, Narrative>();
                    config.CreateMap<Narrative, NarrativeDataModel>();
                    config.CreateMap<TagDataModel, Tag>();
                    config.CreateMap<Tag, TagDataModel>();
                });

            services.AddSingleton<IMapper>(MapConfig.CreateMapper());


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, DbSeeder dbSeeder)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // seed database if needed
            try
            {
                dbSeeder.SeedAsync().Wait();
            }
            catch (AggregateException e)
            {
                throw new Exception(e.ToString());
            }

            app.UseMvc();

        }
    }
}
