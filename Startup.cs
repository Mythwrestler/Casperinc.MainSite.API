using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Introspection;
using Casperinc.MainSite.API.Data;
using Casperinc.MainSite.API.Data.Models;
using Casperinc.MainSite.API.DTOModels;
using Casperinc.MainSite.API.Helpers;
using Casperinc.MainSite.API.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog.Extensions.Logging;

namespace Casperinc.MainSite.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors();

            services.AddDbContext<MainSiteDbContext>(options => {
                options.UseMySql(Configuration["Data:ConnectionStrings:MySQL"]);
            });

            var openIdConfiguation = Configuration.GetSection("OpenIdDict").Get<OpenIdDict>();
            var client = openIdConfiguation.Clients.Where(kvp => kvp.Key == "This").FirstOrDefault().Value;

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = OAuthIntrospectionDefaults.AuthenticationScheme;
            })


            .AddOAuthIntrospection(options =>
            {
                options.Authority = new Uri(openIdConfiguation.IssuingAuthority);
                options.Audiences.Add(client.ClientId);
                options.ClientId = client.ClientId;
                options.ClientSecret = client.ClientSecret;
                options.RequireHttpsMetadata = true;

                // Note: you can override the default name and role claims:
                // options.NameClaimType = "custom_name_claim";
                // options.RoleClaimType = "custom_role_claim";
            });



            services.AddScoped<INarrativeRepository, NarrativeRepository>();

			services.AddScoped<DbSeeder>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper, UrlHelper>(implementationFactory =>
            {
                var actionContext =
                    implementationFactory.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });


            services.AddMvc();

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, DbSeeder dbSeeder)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddNLog();

            if (env.IsDevelopment())
            {
				app.UseDeveloperExceptionPage();

            }
            else
            {
                app.UseExceptionHandler(appBuilder => {
                    appBuilder.Run(async context => {
                        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                        if(exceptionHandlerFeature != null)
                        {
                            var logger = loggerFactory.CreateLogger("Global exception logger");
                            logger.LogError(500, exceptionHandlerFeature.Error, exceptionHandlerFeature.Error.Message);
                        }
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("Unexpected fault. Please try again later.");
                    });
                });
            }


            if (env.IsProduction()) 
            {
                app.UseCors(
                    builder => builder
                                    .WithOrigins("https://www.casperinc.expert")
                                    .WithOrigins("https://dev-web.casperinc.net")
                                    .AllowAnyMethod()
                                    .AllowAnyHeader()
                );
            } else {
                app.UseCors(
                    builder => builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()
                );
            }


            app.UseAuthentication();


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

            AutoMapper.Mapper.Initialize(configure =>
            {
                configure.CreateMap<NarrativeDataModel, NarrativeDTO>();
                configure.CreateMap<NarrativeToCreateDTO, NarrativeDataModel>();
                configure.CreateMap<NarrativeToUpdateDTO, NarrativeDataModel>();
                configure.CreateMap<NarrativeDataModel, NarrativeToUpdateDTO>();
            });


            app.UseMvc();
        }
    }
}
