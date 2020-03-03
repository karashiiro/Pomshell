using Lumina.Excel.GeneratedSheets;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Cyalume = Lumina.Lumina;

namespace Pomshell.Services.GameData
{
    public class LuminaProvider : IGameDataProvider
    {
        private readonly Cyalume _cyalume;

        private readonly static string[] _gameFolders = {
            @"SquareEnix\FINAL FANTASY XIV - A Realm Reborn",
            @"FINAL FANTASY XIV - A Realm Reborn",
            @"SquareEnix\FINAL FANTASY XIV - KOREA",
            @"FINAL FANTASY XIV - KOREA",
            @"上海数龙科技有限公司\最终幻想XIV",
            @"最终幻想XIV",
        };

        private ushort _maxCraftedItemLevel;

        public LuminaProvider()
        {
            foreach (string folder in _gameFolders)
            {
                if (Directory.Exists(Path.Combine(Util.ProgramFilesx86(), folder, @"\game\sqpack")))
                {
                    _cyalume = new Cyalume(folder);
                    break;
                }
            }

            if (_cyalume == null)
            {
                throw new DirectoryNotFoundException("sqpack folder not found!");
            }

            _maxCraftedItemLevel = 0;
        }

        public async Task<ushort> GetMaxCraftedItemLevel()
        {
            if (_maxCraftedItemLevel == 0)
            {
                var items = _cyalume.GetExcelSheet<Item>();
                _maxCraftedItemLevel = items.GetRows()
                    .Where(item => item.CanBeHq)
                    .Max(item => item.LevelItem);
            }
            
            return await Task.FromResult(_maxCraftedItemLevel);
        }
    }
}
