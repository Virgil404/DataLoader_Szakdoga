using DataLoader.Services;
using DataLoader.Services.InterFaces;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;

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
            builder.Services.AddScoped<IJSRuntime, JSRuntime>();
            builder.Services.AddScoped<ICookiesService, Cookieservice>();


            await builder.Build().RunAsync();
        }
    }
}
