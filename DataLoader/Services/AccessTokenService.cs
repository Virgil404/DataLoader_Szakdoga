using DataLoader.Services.InterFaces;

namespace DataLoader.Services
{
    public class AccessTokenService : IAccessTokenService
    {

        private readonly ICookiesService _cookiesService;

        public AccessTokenService(ICookiesService cookiesService)
        {
            _cookiesService = cookiesService;
        }

        public Task<string> GetToken()
        {
            throw new NotImplementedException();
        }

        public Task RemoveToken(string token)
        {
            throw new NotImplementedException();
        }

        public Task SetToken(string token)
        {
            throw new NotImplementedException();
        }
    }
}
