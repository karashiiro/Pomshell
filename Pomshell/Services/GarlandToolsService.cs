using Newtonsoft.Json.Linq;
using Pomshell.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pomshell.Services
{
    public class GarlandToolsService
    {
        private readonly HttpClient _http;

        private static readonly string BASE_URL = "https://www.garlandtools.org";
        private Lang _lang;

        public GarlandToolsService(HttpClient http)
        {
            _http = http;
            _lang = Lang.en;
        }

        public void SetLanguage(Lang lang) => _lang = lang;

        /// <summary>
        /// Gets a list of item IDs of endgame gear pieces for the specified job.
        /// </summary>
        public async Task<IList<ushort>> GetEndgameGear(Job job)
        {
            var ret = new List<ushort>();
            var response = await _http.GetStringAsync(new Uri($"{BASE_URL}/db/doc/equip/{_lang}/2/end-{job}.json"));
            var parsedObj = JObject.Parse(response);
            foreach (var obj in parsedObj["equip"])
            {
                foreach (var item in obj)
                {
                    ret.Add((ushort)item["id"]);
                }
            }
            return ret;
        }
    }
}
