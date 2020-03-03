using System.Net.Http;

namespace Pomshell.Services
{
    public class GarlandToolsService
    {
        private readonly HttpClient _http;

        private string BASE_URL = "https://www.garlandtools.org";
        private Lang _lang;

        public GarlandToolsService(HttpClient http)
        {
            _http = http;
            _lang = Lang.en;
        }

        public void SetLanguage(Lang lang) => _lang = lang;

        public enum Lang
        {
            en,
            fr,
            de,
            ja
        }
    }
}
