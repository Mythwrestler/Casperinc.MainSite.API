using System;
using CasperInc.MainSite.API.Data;
using CasperInc.MainSite.API.Data.Models;
using CasperInc.MainSite.API.Repositories;
using CasperInc.MainSite.API.DTOModels;
using CasperInc.MainSite.Helpers;
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
using Microsoft.IdentityModel.Tokens;
using CryptoHelper;
using System.Threading.Tasks;
using System.Threading;
using OpenIddict.Core;
using OpenIddict.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

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

            //services.AddEntityFrameworkSqlite();

            var mySQLConnectionString = Configuration["connectionStrings:MySQL"];
            var sqLiteConnectionString = Configuration["connectionStrings:SQLite"];
            services.AddDbContext<MainSiteDbContext>(options =>
           {
               options.UseMySql(mySQLConnectionString);
               options.UseOpenIddict();
           });

            services.AddEntityFramework();

            var ConnectionString = Configuration["connectionStrings:MySQL"];

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

            AutoMapper.Mapper.Initialize(configure =>
            {
                configure.CreateMap<NarrativeDataModel, NarrativeDTO>();
                configure.CreateMap<NarrativeToCreateDTO, NarrativeDataModel>();
                configure.CreateMap<NarrativeToUpdateDTO, NarrativeDataModel>();
                configure.CreateMap<NarrativeDataModel, NarrativeToUpdateDTO>();
            });




            app.UseOAuthValidation();


			//app.UseJwtProvider();
			app.UseOpenIddict();

            app.UseJwtBearerAuthentication(new JwtBearerOptions()
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
				RequireHttpsMetadata = false,
				Authority = Configuration["Authentication:OpenIddict:Authority"],
                TokenValidationParameters = new TokenValidationParameters()
                {
                    //IssuerSigningKey = JwtTokenProvider.SecurityKey,
                    //ValidateIssuerSigningKey = true,
                    //ValidIssuer = JwtTokenProvider.Issuer,
                    ValidateIssuer = false,
                    ValidateAudience = false
                }
            });

            app.UseMvc();

			InitializeAsync(app.ApplicationServices, CancellationToken.None).GetAwaiter().GetResult();

        }

        private async Task InitializeAsync(IServiceProvider services, CancellationToken cancellationToken)
		{
			// Create a new service scope to ensure the database context is correctly disposed when this methods returns.
			using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
			{
				var context = scope.ServiceProvider.GetRequiredService<MainSiteDbContext>();
				await context.Database.EnsureCreatedAsync();

				// Note: when using a custom entity or a custom key type, replace OpenIddictApplication by the appropriate type.
				var manager = scope.ServiceProvider.GetRequiredService<OpenIddictApplicationManager<OpenIddictApplication>>();

				if (await manager.FindByClientIdAsync(Configuration["Authentication:OpenIddict:ClientId"], cancellationToken) == null)
				{
                    var application = new OpenIddictApplication
                    {
                        Id = Configuration["Authentication:OpenIddict:ApplicationId"],
                        DisplayName = Configuration["Authentication:OpenIddict:DisplayName"],
                        RedirectUri =  Configuration["Authentication:OpenIddict:Authority"] + Configuration["Authentication:OpenIddict:TokenEndPoint"],
                        LogoutRedirectUri = Configuration["Authentication:OpenIddict:Authority"] + "/",
                        ClientId = Configuration["Authentication:OpenIddict:ClientId"]
					};

					// await manager.CreateAsync(application, Configuration["Authentication:OpenIddict:ClientSecret"], cancellationToken);
					 await manager.CreateAsync(application, cancellationToken);
				}
			}
		}

    }
}
