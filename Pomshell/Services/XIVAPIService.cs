using Newtonsoft.Json;
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
        private static readonly string BASE_URL = "_https://xivapi.com";

        private readonly HttpClient _http;

        public XIVAPIService(HttpClient http)
        {
            _http = http;
        }

        /// <summary>
        /// Gets a character from XIVAPI.
        /// </summary>
        /// <param name="id">The Lodestone ID of the character being queried.</param>
        public async Task<JObject> GetCharacter(ulong id)
            => JObject.Parse(await _http.GetStringAsync(new Uri($"{BASE_URL}/character/{id}?data=AC,MiMo")));

        /// <summary>
        /// Searches for a character on XIVAPI.
        /// </summary>
        /// <param name="name">The character's name.</param>
        /// <param name="server">The character's server.</param>
        public async Task<CharacterSearchResult?> SearchCharacter(string name, string server)
        {
            try
            {
                IList<JToken> generics = JObject.Parse(await _http.GetStringAsync(new Uri($"{BASE_URL}/character/search?name={name}&server={server}")))["Results"]
                    .Children()
                    .ToList();
                return (generics as IList<CharacterSearchResult>).Single(result => result.Name == name && result.Server == server);
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        /// <summary>
        /// Searches for a linkshell on XIVAPI.
        /// </summary>
        /// <param name="name">The name of the linkshell.</param>
        /// <param name="server">The server of the linkshell.</param>
        public async Task<LinkshellSearchResult> SearchLinkshell(string name, string server)
        {
            var res = (IList<LinkshellSearchResult>)JObject.Parse(await _http.GetStringAsync(new Uri($"{BASE_URL}/linkshell/search?name={name}")))["Results"]
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

        /// <summary>
        /// Searches for a CWLS on XIVAPI.
        /// </summary>
        /// <param name="name">The name of the CWLS.</param>
        /// <param name="server">The server of the CWLS.</param>
        public async Task<LinkshellSearchResult> SearchCWLS(string name, string server)
        {
            var res = (IList<LinkshellSearchResult>)JObject.Parse(await _http.GetStringAsync(new Uri($"{BASE_URL}/linkshell/crossworld/search?name={name}")))["Results"]
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

    public struct CharacterSearchResult
    {
        public string Avatar;
        public int FeastMatches;
        public ulong ID;
        public string Lang;
        public string Name;
        public object Rank;
        public object RankIcon;
        public string Server;
    }
}
