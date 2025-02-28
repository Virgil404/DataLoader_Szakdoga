using Blazored.LocalStorage;
using Dataloader.Api.DTO;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json.Nodes;

namespace Identity_auth.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {

        private readonly HttpClient _httpClient;
        private readonly ISyncLocalStorageService _localStorageService;

        public CustomAuthStateProvider(HttpClient httpClient, ISyncLocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;

            var token = _localStorageService.GetItem<string>("accessToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }


        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity());

            try
            {
                var response = await _httpClient.GetAsync("/api/Auth/userprofile");

                if (response.IsSuccessStatusCode)
                {

                    /*
                    var stringresp = await response.Content.ReadAsStringAsync();
                    var jsonresp = JsonNode.Parse(stringresp);
                    var username = jsonresp["email"].ToString();
                    //  var role = jsonresp["role"].ToString();
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, username),
                        new Claim(ClaimTypes.Email, username)
                    };
                    */
                    var userprofile = await response.Content.ReadFromJsonAsync<UserDTO>();
                    if (userprofile == null) throw new Exception("User not found");
                    var userroles = userprofile.Role.Split(",");
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, userprofile.username),
                        new Claim(ClaimTypes.Email, userprofile.email)
                    };

                    foreach (var role in userroles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    var identity = new ClaimsIdentity(claims, "Token");
                    user = new ClaimsPrincipal(identity);
                    return new AuthenticationState(user);
                }

            }
            catch (Exception)
            {


            }

            return new AuthenticationState(user);

        }

        public async Task<bool> LoginAsync(string username, string password)
        {

            try
            {
                var json = new
                {
                    email = username,
                    password = password
                };

                var response = await _httpClient.PostAsJsonAsync("login", json);

                if (response.StatusCode.Equals("401"))
                {
                    return false;
                }
               
                if (response.IsSuccessStatusCode)
                {
                    var stringresp = await response.Content.ReadAsStringAsync();
                    var jsonresp = JsonNode.Parse(stringresp);
                    var accesstoken = jsonresp["accessToken"].ToString();
                    var refreshToken = jsonresp["refreshToken"].ToString();

                    _localStorageService.SetItem("accessToken", accesstoken);
                    _localStorageService.SetItem("refreshToken", refreshToken);

                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);

                    NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());


                    return true;
                }
                else
                {
                    //var errors = await response.Content.ReadFromJsonAsync<string[]>();
                    return false;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Logout()
        {
            _localStorageService.RemoveItem("accessToken");
            _localStorageService.RemoveItem("refreshToken");
            _httpClient.DefaultRequestHeaders.Authorization = null;

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());


        }
    }


    public class FormResult
    {

        public bool succeeded { get; set; }

        public string[] Errors { get; set; } = [];
    }
}
