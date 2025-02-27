using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DataLoader.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace DataLoader.Security
{
    public class JWTAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly AccessTokenService _tokenService; 

        public JWTAuthenticationStateProvider(AccessTokenService tokenService)
        {
            _tokenService = tokenService;
        }


        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {

            try
            {
                var token = await _tokenService.GetToken();

                if (String.IsNullOrWhiteSpace(token) )
                {

                   return await MarkAsUnauthorized();
                }
               
                var readJWT = new  JwtSecurityTokenHandler().ReadJwtToken(token);
                var identity = new ClaimsIdentity(readJWT.Claims, "JWT");
                var principal = new ClaimsPrincipal(identity);

                return await Task.FromResult( new AuthenticationState(principal));

            }
            catch (Exception)
            {

              return await  MarkAsUnauthorized();
            }
        }

        private async Task<AuthenticationState> MarkAsUnauthorized() 
        {

            try
            {

                var state = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                NotifyAuthenticationStateChanged(Task.FromResult(state));

                return state;

            }
            catch (Exception)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
               
            }


        }
    }
}
