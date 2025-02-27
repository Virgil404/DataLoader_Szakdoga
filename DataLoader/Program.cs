using DataLoader.Security;
using DataLoader.Services;
using DataLoader.Services.InterFaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using Radzen;

namespace DataLoader
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7186") });
            builder.Services.AddScoped<ITaskSchedulerService, TaskSchedulerService>();
            builder.Services.AddScoped<ICookiesService, Cookieservice>();
            builder.Services.AddScoped<AccessTokenService>(); 
            builder.Services.AddScoped<IAccessTokenService>(sp => sp.GetRequiredService<AccessTokenService>());
            builder.Services.AddScoped<IUserManagerService, UserManagerService>();
            builder.Services.AddScoped<AuthService>();  
            builder.Services.AddScoped<IAuthService, AuthService>();
         //   builder.Services.AddScoped<DialogService>();
           
            builder.Services.AddApiAuthorization();
            builder.Services.AddAuthentication()
                .AddScheme<CustomOption,JWTAuthenticaionHandler>
                (
                "JWTAuth", opt => { }
                );
            builder.Services.AddScoped<JWTAuthenticationStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider, JWTAuthenticationStateProvider>();
            builder.Services.AddRadzenComponents();
            builder.Services.AddCascadingAuthenticationState();

            await builder.Build().RunAsync();
        }
    }
}
