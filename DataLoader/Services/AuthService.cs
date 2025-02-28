using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Web;
using Dataloader.Api.DTO;
using DataLoader.Services.InterFaces;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace DataLoader.Services
{
    public class AuthService :IAuthService
    {
      //  private readonly IAccessTokenService _accessTokenService;
        private readonly NavigationManager _navigationManager;
        private readonly HttpClient _httpClient;
        public AuthService(NavigationManager navigationManager, HttpClient httpClient
            ) 
        {
            //_accessTokenService = accessTokenService;
            _navigationManager = navigationManager;
            _httpClient = httpClient;

        }

        public async Task<bool> Login(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
