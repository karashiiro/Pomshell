using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Pomshell.Services
{
    public class XIVAPIService
    {
        private static readonly string BASE_URL = "https://xivapi.com";

        private readonly HttpClient http;

        public XIVAPIService(HttpClient httpClient)
        {
            http = httpClient;
        }

        public async Task<JObject> GetCharacter(ulong id)
            => JObject.Parse(await http.GetStringAsync(new Uri($"{BASE_URL}/character/{id}?data=AC,MiMo")));

        public async Task<LinkshellSearchResult> SearchLinkshell(string name, string server)
        {
            var res = (IList<LinkshellSearchResult>)JObject.Parse(await http.GetStringAsync(new Uri($"{BASE_URL}/linkshell/search?name={name}")))["Results"]
                .Children()
                .ToList();
            foreach (var entry in res)
            {
                if (entry.Name.ToLower() == name && entry.Server.StartsWith(server))
                {
                    return entry;
                }
            }
            throw new XIVAPIDataNotFoundException();
        }

        public async Task<LinkshellSearchResult> SearchCWLS(string name, string server)
        {
            var res = (IList<LinkshellSearchResult>)JObject.Parse(await http.GetStringAsync(new Uri($"{BASE_URL}/linkshell/crossworld/search?name={name}")))["Results"]
                .Children()
                .ToList();
            foreach (var entry in res)
            {
                if (entry.Name.ToLower() == name && entry.Server == server)
                {
                    return entry;
                }
            }
            throw new XIVAPIDataNotFoundException();
        }
    }

    [Serializable]
    internal class XIVAPIDataNotFoundException : Exception
    {
        public XIVAPIDataNotFoundException() {}
        public XIVAPIDataNotFoundException(string message) : base(message) {}
        public XIVAPIDataNotFoundException(string message, Exception innerException) : base(message, innerException) {}
        protected XIVAPIDataNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) {}
    }

    public struct LinkshellSearchResult
    {
        public object Crest;
        public string ID;
        public string Name;
        public string Server;
    }
}
