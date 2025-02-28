using Blazored.LocalStorage;
using DataLoader.Services;
using DataLoader.Services.InterFaces;
using Identity_auth.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
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
 
            builder.Services.AddScoped<IUserManagerService, UserManagerService>();
            builder.Services.AddScoped<AuthService>();  
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
            //   builder.Services.AddScoped<DialogService>();

            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddApiAuthorization();
         
            builder.Services.AddRadzenComponents();
            builder.Services.AddCascadingAuthenticationState();

            await builder.Build().RunAsync();
        }
    }
}
