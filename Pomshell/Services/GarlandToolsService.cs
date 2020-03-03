using System.Net.Http;

namespace Pomshell.Services
{
    public class GarlandToolsService
    {
        private readonly HttpClient _http;

        public GarlandToolsService(HttpClient http)
        {
            _http = http;
        }
    }
}
