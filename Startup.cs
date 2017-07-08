using System;
using Casperinc.MainSite.API.Data;
using Casperinc.MainSite.API.Data.Models;
using Casperinc.MainSite.API.Repositories;
using Casperinc.MainSite.API.DTOModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Casperinc.MainSite.API
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("dbConnections.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors();

            // Add framework services.
            services.AddMvc(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
                setupAction.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                setupAction.InputFormatters.Add(new XmlDataContractSerializerInputFormatter());
            });

            services.AddEntityFrameworkSqlite();

            var ConnectionString = Configuration["connectionStrings:MySQL"];

            services.AddDbContext<MainSiteDbContext>(
                //options => options.UseSqlite(ConnectionString)
                options => options.UseMySql(ConnectionString)
            );

            services.AddScoped<INarrativeRepository, NarrativeRepository>();

			services.AddSingleton<DbSeeder>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper, UrlHelper>(implementationFactory =>
            {
                var actionContext =
                    implementationFactory.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            }
            
            );

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, DbSeeder dbSeeder)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddNLog();

            if (env.IsProduction())
            {
                app.UseCors(
                    builder => builder.WithOrigins("https://www.casperinc.expert")
                                    .AllowAnyMethod()
                                    .AllowAnyHeader()
                );
            }
            else
            {
                app.UseCors(
                    builder => builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()
                );
            }

            AutoMapper.Mapper.Initialize(configure =>
            {
                configure.CreateMap<NarrativeTagDataModel, narativeTags>();
                configure.CreateMap<TagDataModel, TagDTO>();
                configure.CreateMap<NarrativeDataModel, NarrativeDTO>();
                configure.CreateMap<NarrativeToCreateDTO, NarrativeDataModel>();
                configure.CreateMap<TagToCreateDTO, TagDataModel>();
            });

            // seed database if needed
            try
            {
                if(env.IsDevelopment())
                {
                    dbSeeder.ResetDBAsync().Wait();
                }else{
                    dbSeeder.SeedAsync().Wait();
                }
            }
            catch (AggregateException e)
            {
                throw new Exception(e.ToString());
            }

            app.UseMvc();

        }
    }
}
