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
        private readonly IAccessTokenService _accessTokenService;
        private readonly NavigationManager _navigationManager;
        private readonly HttpClient _httpClient;
        public AuthService(IAccessTokenService accessTokenService, NavigationManager navigationManager, HttpClient httpClient
            ) 
        {
            _accessTokenService = accessTokenService;
            _navigationManager = navigationManager;
            _httpClient = httpClient;

        }

        public async Task<bool> Login(string username, string password)
        {

            var url = $"/api/Auth/Login?username={Uri.EscapeDataString(username)}&password ={Uri.EscapeDataString(password)}";

            var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");


            var status = await _httpClient.PostAsync(url,content);

            if (status.IsSuccessStatusCode)
            {
                var token = await status.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<AuthResponse>(token);
                await _accessTokenService.SetToken(result.AccesToken);
                return true; 

            }
            else return false;


        }
    }
}
