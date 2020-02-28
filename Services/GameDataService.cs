using Lumina.Excel.GeneratedSheets;
using Microsoft.VisualBasic.FileIO;
using System;
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

        private readonly static string[] sqpack = {
            @"SquareEnix\FINAL FANTASY XIV - A Realm Reborn",
            @"FINAL FANTASY XIV - A Realm Reborn",
            @"SquareEnix\FINAL FANTASY XIV - KOREA",
            @"FINAL FANTASY XIV - KOREA",
            @"上海数龙科技有限公司\最终幻想XIV",
            @"最终幻想XIV",
        };

        public GameDataService(HttpClient http)
        {
            foreach (string folder in sqpack)
            {
                if (Directory.Exists(Path.Combine(ProgramFilesx86(), folder, @"\game\sqpack")))
                {
                    _cyalume = new Cyalume(folder);
                    FoundSqpack = true;
                    break;
                }
            }
            _http = http;
        }

        /// <summary>
        /// Gets the highest crafted battle gear item level.
        /// </summary>
        public async Task<ushort> GetMaxCraftedItemLevel()
        {
            #region PC
            if (FoundSqpack)
            {
                var items = _cyalume.GetExcelSheet<Item>();
                return items.GetRows()
                    .Where(item => item.CanBeHq)
                    .Max(item => item.LevelItem);
            }
            #endregion
            #region PS4
            else
            {
                using var parser = new TextFieldParser(await _http.GetStringAsync(new Uri("https://raw.githubusercontent.com/xivapi/ffxiv-datamining/master/csv/Item.csv")));
                parser.SetDelimiters(",");

                ushort lastItemId = 0;
                ushort lastLevelItem = 0;

                for (int i = 0; i < 3; i++) parser.ReadFields(); // Get rid of the headers. Row 0 is the index row, row 1 is the field name row, and row 2 is the type row.
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    ushort itemId = ushort.Parse(fields[0]);
                    ushort levelItem = ushort.Parse(fields[12]);
                    bool canBeHq = Convert.ToBoolean(byte.Parse(fields[26]));

                    if (canBeHq && levelItem > lastLevelItem)
                    {
                        lastItemId = itemId;
                        lastLevelItem = levelItem;
                    }
                }

                return lastItemId;
            }
            #endregion
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
