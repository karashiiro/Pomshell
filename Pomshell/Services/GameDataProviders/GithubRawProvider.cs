using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Table = System.Collections.Generic.List<System.Collections.Generic.List<string>>;

namespace Pomshell.Services.GameData
{
    public class GithubRawProvider : IGameDataProvider
    {
        private readonly HttpClient _http;

        private readonly IDictionary<string, Table> _tables;

        private ushort _maxCraftedItemLevel;

        public GithubRawProvider(HttpClient http)
        {
            _http = http;

            _tables = new Dictionary<string, Table>();

            _maxCraftedItemLevel = 0;
        }

        public async Task<ushort> GetMaxCraftedItemLevel()
        {
            if (_maxCraftedItemLevel == 0)
            {
                Table table = await GetTable("https://raw.githubusercontent.com/xivapi/ffxiv-datamining/master/csv/Item.csv");
                _maxCraftedItemLevel = table
                    .Where(row => Convert.ToBoolean(row[26]))
                    .Max(row => ushort.Parse(row[12]));
            }

            return await Task.FromResult(_maxCraftedItemLevel);
        }

        public async Task<ushort> GetItemLevel(ushort itemId)
        {
            Table table = await GetTable("https://raw.githubusercontent.com/xivapi/ffxiv-datamining/master/csv/Item.csv");
            ushort id = 0;
            var t = table.Where(row => ushort.TryParse(row[0], out id) && id == itemId);
            return t.Any() ? id : new ushort();
        }

        /// <summary>
        /// Requests a table, parses it, and caches it in the dictionary.
        /// </summary>
        private async Task<Table> GetTable(string uri)
        {
            if (!_tables.ContainsKey(uri))
            {
                var response = await _http.GetStringAsync(new Uri(uri));
                string[] rows = response.Split('\n');
                Table table = new Table();
                foreach(string row in rows)
                {
                    table.Add(row.Split(',').ToList());
                }
                _tables.Add(uri, table);
            }
            return _tables[uri];
        }
    }
}