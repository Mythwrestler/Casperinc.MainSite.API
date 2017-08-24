using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using AspNet.Security.OpenIdConnect.Server;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Core;
using OpenIddict.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OpenGameList.Controllers
{
    [Route("/mainsite/api/connect")]
    public class ConnectController : Controller
    {
        private OpenIddictApplicationManager<OpenIddictApplication> _applicationManager;

        public ConnectController(OpenIddictApplicationManager<OpenIddictApplication> applicationManager)
        {
            _applicationManager = applicationManager;
        }


        [HttpPost("token")]
        public async Task<IActionResult> Exchange(OpenIdConnectRequest request)
        {
            
            Debug.Assert(request.IsTokenRequest()," ");

            if (request.IsPasswordGrantType())
            {
                var application = await _applicationManager.FindByClientIdAsync(request.ClientId, HttpContext.RequestAborted);

                if (application == null)
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidClient,
                        ErrorDescription = "The client was not found in the database."
                    });
                }

                var ticket = CreateTicket(request, application);

                return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);

            }


			return BadRequest(new OpenIdConnectResponse
			{
				Error = OpenIdConnectConstants.Errors.UnsupportedGrantType,
				ErrorDescription = "The specified grant type is not supported."
			});   

        }

		private AuthenticationTicket CreateTicket(OpenIdConnectRequest request, OpenIddictApplication application)
		{
			// Create a new ClaimsIdentity containing the claims that
			// will be used to create an id_token, a token or a code.
			var identity = new ClaimsIdentity(
				OpenIdConnectServerDefaults.AuthenticationScheme,
				OpenIdConnectConstants.Claims.Name,
				OpenIdConnectConstants.Claims.Role);

			// Use the client_id as the subject identifier.
			identity.AddClaim(OpenIdConnectConstants.Claims.Subject, application.ClientId,
				OpenIdConnectConstants.Destinations.AccessToken,
				OpenIdConnectConstants.Destinations.IdentityToken);

			identity.AddClaim(OpenIdConnectConstants.Claims.Name, application.DisplayName,
				OpenIdConnectConstants.Destinations.AccessToken,
				OpenIdConnectConstants.Destinations.IdentityToken);

			identity.AddClaim(OpenIdConnectConstants.Claims.Username, request.Username,
				OpenIdConnectConstants.Destinations.AccessToken,
				OpenIdConnectConstants.Destinations.IdentityToken);


			// Create a new authentication ticket holding the user identity.
			var ticket = new AuthenticationTicket(
				new ClaimsPrincipal(identity),
				new AuthenticationProperties(),
				OpenIdConnectServerDefaults.AuthenticationScheme);

			ticket.SetResources("resource_server");

			return ticket;
		}


	}
}
