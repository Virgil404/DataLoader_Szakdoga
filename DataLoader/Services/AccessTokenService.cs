using DataLoader.Services.InterFaces;

namespace DataLoader.Services
{
    public class AccessTokenService : IAccessTokenService
    {

        private readonly ICookiesService _cookiesService;
        private readonly string tokenkey="access_token";

        public AccessTokenService(ICookiesService cookiesService)
        {
            _cookiesService = cookiesService;
        }

        public async Task<string> GetToken()
        {
            return await _cookiesService.GetCookie(tokenkey);
        }

        public async Task RemoveToken(string token)
        {
        
            await _cookiesService.RemoveCookie(tokenkey);
        
        }

        public async Task SetToken(string token)
        {
             await _cookiesService.SetCookie(tokenkey, token, 1);
        }
    }
}
