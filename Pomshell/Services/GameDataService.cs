using Lumina.Excel.GeneratedSheets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Cyalume = Lumina.Lumina;

namespace Pomshell.Services
{
    public class GameDataService
    {
        public bool FoundSqpack { get; private set; } = false;

        private readonly Cyalume _cyalume;
        private readonly HttpClient _http;

        private ushort _maxCraftedItemLevel;

        private readonly static string[] _gameFolders = {
            @"SquareEnix\FINAL FANTASY XIV - A Realm Reborn",
            @"FINAL FANTASY XIV - A Realm Reborn",
            @"SquareEnix\FINAL FANTASY XIV - KOREA",
            @"FINAL FANTASY XIV - KOREA",
            @"上海数龙科技有限公司\最终幻想XIV",
            @"最终幻想XIV",
        };

        public GameDataService(HttpClient http)
        {
            foreach (string folder in _gameFolders)
            {
                if (Directory.Exists(Path.Combine(ProgramFilesx86(), folder, @"\game\sqpack")))
                {
                    _cyalume = new Cyalume(folder);
                    FoundSqpack = true;
                    break;
                }
            }
            _http = http;

            _maxCraftedItemLevel = 0;
        }

        /// <summary>
        /// Gets the highest crafted battle gear item level.
        /// </summary>
        public async Task<ushort> GetMaxCraftedItemLevel()
        {
            if (_maxCraftedItemLevel != 0)
            {
                return _maxCraftedItemLevel;
            }
            #region PC
            if (FoundSqpack)
            {
                var items = _cyalume.GetExcelSheet<Item>();
                _maxCraftedItemLevel = items.GetRows()
                    .Where(item => item.CanBeHq)
                    .Max(item => item.LevelItem);
            }
            #endregion
            #region PS4
            else
            {
                // I'd rather not make a ton of XIVAPI requests and have to deal with rate-limiting, this is only run once, anyways.
                var response = await _http.GetStringAsync(new Uri("https://raw.githubusercontent.com/xivapi/ffxiv-datamining/master/csv/Item.csv"));
                string[] rows = response.Split('\n');
                List<string[]> result = new List<string[]>();
                foreach(string row in rows)
                {
                    result.Add(row.Split('\n'));
                }
                _maxCraftedItemLevel = result
                    .Where(row => Convert.ToBoolean(row[26]))
                    .Max(row => ushort.Parse(row[12]));
            }
            #endregion
            return _maxCraftedItemLevel;
        }

        private static string ProgramFilesx86()
        {
            if (Environment.Is64BitOperatingSystem)
            {
                return Environment.GetEnvironmentVariable("ProgramFiles(x86)") ?? @"C:\Program Files (x86)";
            }

            return Environment.GetEnvironmentVariable("ProgramFiles") ?? @"C:\Program Files";
        }
    }
}
