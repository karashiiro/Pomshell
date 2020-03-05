using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Pomshell.Services;
using System.Net.Http;

namespace Pomshell.Blazor.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("Pomshell");

            builder.Services
                .AddSingleton<GameDataService>()
                .AddSingleton<XIVAPIService>();

            await builder.Build().RunAsync();
        }
    }
}
