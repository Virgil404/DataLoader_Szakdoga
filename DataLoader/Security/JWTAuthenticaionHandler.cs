using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Options;

namespace DataLoader.Security
{
    public class JWTAuthenticaionHandler : AuthenticationHandler<CustomOption>
    {
        public JWTAuthenticaionHandler(IOptionsMonitor<CustomOption> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                var token = Request.Cookies["access_token"];

                if (String.IsNullOrEmpty(token)) {

                   return  AuthenticateResult.NoResult();
                    
                }


                var readJWT = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var identity = new ClaimsIdentity(readJWT.Claims, "JWT");
                var principal = new ClaimsPrincipal(identity);         
                var ticket =  new AuthenticationTicket(principal,Scheme.Name);

                return  AuthenticateResult.Success(ticket); 

            }
            catch (Exception)
            {

                return AuthenticateResult.NoResult();
            }


        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.Redirect("/login");
            return Task.CompletedTask;
        }

        protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        {

            Response.Redirect("/accesdenied");
            return Task.CompletedTask;
        }


    }

    public class CustomOption : AuthenticationSchemeOptions
    {


    }
}
