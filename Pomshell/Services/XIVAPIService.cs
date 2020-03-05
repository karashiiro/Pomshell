using Newtonsoft.Json.Linq;
using Pomshell.Models;
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

        private readonly HttpClient _http;

        public XIVAPIService(HttpClient http)
            => _http = http;

        /// <summary>
        /// Returns the release dates of a set of items.
        /// </summary>
        public async Task<IDictionary<ushort, long>> GetItemsReleaseDates(IEnumerable<ushort> items)
        {
            var output = new Dictionary<ushort, long>();

            var query = "https://xivapi.com/Item?ids=";
            query += items.Select(item => item.ToString()).Aggregate((item1, item2) => item1 + ',' + item2);
            query += "&columns=ID,GamePatch.ReleaseDate";

            int pageTotal = 1;
            for (int i = 1; i < pageTotal; i++)
            {
                var response = JObject.Parse(await _http.GetStringAsync(new Uri($"query&page={i}")));
                pageTotal = (int)response["Pagination"]["PageTotal"];
                foreach (var child in response["Results"].Children())
                {
                    output.Add((ushort)child["ID"], (long)child["GamePatch"]["ReleaseDate"] * 1000);
                }
            }

            return output;
        }

        /// <summary>
        /// Gets the XIVAPI patch list.
        /// </summary>
        public async Task<IList<XIVAPIPatchList>> GetPatches()
            => JObject.Parse(await _http.GetStringAsync(new Uri($"{BASE_URL}/patchlist"))).Children().ToList() as IList<XIVAPIPatchList>;

        /// <summary>
        /// Gets a character from XIVAPI.
        /// </summary>
        /// <param name="id">The Lodestone ID of the character being queried.</param>
        public async Task<JObject> GetCharacter(ulong id)
            => JObject.Parse(await _http.GetStringAsync(new Uri($"{BASE_URL}/character/{id}?data=AC,MiMo")));

        /// <summary>
        /// Gets a linkshell's members from XIVAPI.
        /// </summary>
        /// <param name="id">The Lodestone ID of the linkshell being queried.</param>
        public async Task<IList<CharacterSearchResult>> GetLinkshellMembers(ulong id)
        {
            var generics = Enumerable.Empty<JToken>();
            int pageTotal = 1;
            for (int i = 1; i < pageTotal; i++)
            {
                JToken res = JObject.Parse(await _http.GetStringAsync(new Uri($"{BASE_URL}/linkshell/{id}?page={i}")))["Linkshell"];
                pageTotal = (int)res["Pagination"]["PageTotal"];
                generics.Concat(res["Results"].Children());
            }
            // Distinct() handles the edge case in which the list is updated as it's being fetched.
            return generics.Distinct().ToList() as IList<CharacterSearchResult>;
        }

        /// <summary>
        /// Gets a CWLS's members from XIVAPI.
        /// </summary>
        /// <param name="id">The Lodestone ID of the CWLS being queried.</param>
        public async Task<IList<CharacterSearchResult>> GetCWLSMembers(ulong id)
        {
            var generics = Enumerable.Empty<JToken>();
            int pageTotal = 1;
            for (int i = 1; i < pageTotal; i++)
            {
                JToken res = JObject.Parse(await _http.GetStringAsync(new Uri($"{BASE_URL}/linkshell/crossworld/{id}?page={i}")))["Linkshell"];
                pageTotal = (int)res["Pagination"]["PageTotal"];
                generics.Concat(res["Results"].Children());
            }
            return generics.Distinct().ToList() as IList<CharacterSearchResult>;
        }

        /// <summary>
        /// Searches for a character on XIVAPI.
        /// </summary>
        /// <param name="name">The character's name.</param>
        /// <param name="server">The character's server.</param>
        public async Task<CharacterSearchResult> SearchCharacter(string name, string server)
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
                throw new XIVAPIDataNotFoundException();
            }
        }

        /// <summary>
        /// Searches for a linkshell on XIVAPI.
        /// </summary>
        /// <param name="name">The name of the linkshell.</param>
        /// <param name="server">The server of the linkshell.</param>
        public async Task<LinkshellSearchResult> SearchLinkshell(string name, string server)
        {
            var res = (IList<LinkshellSearchResult>)JObject.Parse(await _http.GetStringAsync(new Uri($"{BASE_URL}/linkshell/search?name={name}&server={server}")))["Results"]
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
            var res = (IList<LinkshellSearchResult>)JObject.Parse(await _http.GetStringAsync(new Uri($"{BASE_URL}/linkshell/crossworld/search?name={name}&server={server}")))["Results"]
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
}
