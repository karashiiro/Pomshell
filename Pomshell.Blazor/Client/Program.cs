using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Pomshell.Services;
using Pomshell.Storage;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace Pomshell.Blazor.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddBaseAddressHttpClient();

            builder.Services.AddBlazoredLocalStorage();
            // This loading bar is kind of cute
            builder.Services.AddLoadingBar();

            builder.Services
                .AddSingleton<ICacheLayer, MemoryOnlyCacheLayer>()
                .AddSingleton<GameDataService>()
                .AddSingleton<XIVAPIService>();
            
            await builder
                .Build()
                .UseLoadingBar()
                .UseLocalTimeZone()
                .RunAsync();
        }
    }
}
