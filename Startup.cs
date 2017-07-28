﻿using System;
using CasperInc.MainSite.API.Data;
using CasperInc.MainSite.API.Data.Models;
using CasperInc.MainSite.API.Repositories;
using CasperInc.MainSite.API.DTOModels;
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
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CasperInc.MainSite.API
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


            services.AddIdentity<UserDataModel, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Cookies.ApplicationCookie.AutomaticChallenge = false;
            })
            .AddEntityFrameworkStores<MainSiteDbContext>()
            .AddDefaultTokenProviders();


            // Register the OpenIddict services.
            // Note: use the generic overload if you need
            // to replace the default OpenIddict entities.
            services.AddOpenIddict(options =>
            {
                // Register the Entity Framework stores.
                options.AddEntityFrameworkCoreStores<MainSiteDbContext>();

                options.UseJsonWebTokens();

                // Register the ASP.NET Core MVC binder used by OpenIddict.
                // Note: if you don't call this method, you won't be able to
                // bind OpenIdConnectRequest or OpenIdConnectResponse parameters.
                options.AddMvcBinders();

				// Enable the token endpoint (required to use the password flow).
				options.EnableTokenEndpoint(
						Configuration["Authentication:OpenIddict:TokenEndPoint"]
                );

				options.EnableAuthorizationEndpoint(
						Configuration["Authentication:OpenIddict:AuthorizationEndPoint"]
				);

                // Allow client applications to use the grant_type=password flow.
                options.AllowPasswordFlow();

                options.AllowAuthorizationCodeFlow();

                options.AllowImplicitFlow();

                options.AllowRefreshTokenFlow();

                // During development, you can disable the HTTPS requirement.
                options.DisableHttpsRequirement();

                options.AddEphemeralSigningKey();
            });



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
            loggerFactory.AddConsole();
            //loggerFactory.AddDebug();
            loggerFactory.AddNLog();

            if (env.IsProduction()) 
            {
                app.UseCors(
                    builder => builder.WithOrigins("https://www.CasperInc.expert")
                                    .AllowAnyMethod()
                                    .AllowAnyHeader()
                );
            } else {
                app.UseCors(
                    builder => builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()
                );
            }

            AutoMapper.Mapper.Initialize(configure =>
            {
                configure.CreateMap<NarrativeDataModel, NarrativeDTO>();
                configure.CreateMap<NarrativeToCreateDTO, NarrativeDataModel>();
                configure.CreateMap<NarrativeToUpdateDTO, NarrativeDataModel>();
                configure.CreateMap<NarrativeDataModel, NarrativeToUpdateDTO>();
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
