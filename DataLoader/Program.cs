using DataLoader.Services;
using DataLoader.Services.InterFaces;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

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


            await builder.Build().RunAsync();
        }
    }
}
