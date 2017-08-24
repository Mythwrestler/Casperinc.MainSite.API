using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using CasperInc.MainSite.API.Data;
using CasperInc.MainSite.API.Data.Models;
using Microsoft.Extensions.Configuration;

namespace CasperInc.MainSite.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class JwtTokenProvider
    {

        #region private members

        private readonly RequestDelegate _next;

        //Jwt-related Members
        private TimeSpan TokenExpiration;
        private SigningCredentials SigningCredentials;

        // EF and Identity members, available through DI
        private MainSiteDbContext _dbContext;
        private UserManager<UserDataModel> _userManager;
        private SignInManager<UserDataModel> _signInManager;


        #endregion

        #region static members

        private static readonly string PrivateKey = "private_key_1234567890";
        public static readonly SymmetricSecurityKey SecurityKey =
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(PrivateKey));
        public static readonly string Issuer = "Casperinc.MainSite.API";
        public static string TokenEndPoint = "/mainsite/api/connect/token";
        private IConfigurationRoot _configuration;

        #endregion

        #region public methods

        public JwtTokenProvider(
            RequestDelegate next,
            MainSiteDbContext dbContext,
            UserManager<UserDataModel> userManager,
            SignInManager<UserDataModel> signInManager,
            IConfigurationRoot configuration
        )
        {
            _next = next;

            //instantiate JWT-related memebers;
            TokenExpiration = TimeSpan.FromMinutes(10);
            SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);

            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;

            _configuration = configuration;

        }



        public Task Invoke(HttpContext httpContext)
        {

            // Check if the request path matches our token endpoint
            if (!httpContext.Request.Path.Equals(TokenEndPoint, StringComparison.Ordinal))
            {
                return _next(httpContext);
            }

            // Check if the current request is a valid POST with the appropriate content type
            // (application/x-www-form-urlencoded)
            if (httpContext.Request.Method.Equals("POST") && httpContext.Request.HasFormContentType)
            {
                return CreateToken(httpContext);
            }
            else
            {

                //not OK output a 400 bad request http error.
                httpContext.Response.StatusCode = 400;
                return httpContext.Response.WriteAsync("Bad Request.");
            }

        }

        #endregion

        #region private methods

        private async Task CreateToken(HttpContext httpContext)
        {
            try
            {
                //retrieve the relevant form data
                string username = httpContext.Request.Form["username"];
                string password = httpContext.Request.Form["password"];

                var user = await _userManager.FindByNameAsync(username);
                if (user == null && username.Contains("@")) user = await _userManager.FindByEmailAsync(username);

                var success = ((user != null) &&
                               await _userManager.CheckPasswordAsync(user, password)
                              );
                if (success)
                {
                    DateTime now = DateTime.UtcNow;

                    //add the registerd claims for JWt
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Iss, Issuer),
                        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
                        // TODO: Add additional claims here
                    };

                    // Create the JWT and write it to a string
                    var token = new JwtSecurityToken(
                            claims: claims,
                            notBefore: now,
                            expires: now.Add(TokenExpiration),
                            signingCredentials: SigningCredentials
                    );

                    var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);

                    //build json response
                    var jwt = new
                    {
                        access_token = encodedToken,
                        expiration = (int)TokenExpiration.TotalSeconds
                    };

                    // return token
                    httpContext.Response.ContentType = "application/json";

                    await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(jwt));

                    return;
                }

            }
            catch (Exception ex)
            {
                // TODO: handle errors
                throw ex;
            }


            httpContext.Response.StatusCode = 400;
            await httpContext.Response.WriteAsync("Invalid Username or Password.");

        }

        #endregion

    }


    #region Extension Methods Region

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class JwtTokenProviderExtensions
    {
        public static IApplicationBuilder UseJwtProvider(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtTokenProvider>();
        }
    }

    #endregion

}
