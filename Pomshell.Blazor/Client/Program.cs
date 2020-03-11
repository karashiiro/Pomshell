using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Pomshell.Services;

namespace Pomshell.Blazor.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services
                .AddSingleton<GameDataService>()
                .AddSingleton<XIVAPIService>();

            await builder.Build().RunAsync();
        }
    }
}
