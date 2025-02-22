using DataLoader.Services.InterFaces;
using Microsoft.JSInterop;

namespace DataLoader.Services
{
    public class Cookieservice : ICookiesService
    {

        private readonly IJSRuntime _runtime;
       // private readonly HttpClient _httpClient;
        public Cookieservice( HttpClient httpClient, IJSRuntime runtime) 
        {
           // _httpClient = httpClient;
            _runtime = runtime;
        }

        public async Task<string> GetCookie(string key)
        {
            return await _runtime.InvokeAsync<string>("getCookie",key);
        }

        public async Task RemoveCookie(string key)
        {
             await _runtime.InvokeVoidAsync("deleteCookie",key);
        }

        public async Task SetCookie(string key, string value, int days)
        {
             await _runtime.InvokeVoidAsync("setCookie",key,value,days);
        }
    }
}
