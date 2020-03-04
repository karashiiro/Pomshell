using Pomshell.Services.GameData;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pomshell.Services
{
    public class GameDataService : IGameDataProvider
    {
        // Used for frontend UI elements.
        public bool FoundSqpack { get; private set; } = false;

        /*
         * I really don't want to deal with XIVAPI's slow speeds and rate-limiting, so in the event that exd
         * data isn't available, we'll just parse the chonky CSV files instead. Sucks, but it's still faster
         * than XIVAPI, and that load time only needs to be dealt with once rather than throughout the entire
         * application use period.
         */
        private IGameDataProvider _dataProvider;

        private readonly HttpClient _http;

        public GameDataService(HttpClient http)
        {
            _http = http;
            ReloadProvider();
        }

        public async Task<ushort> GetMaxCraftedItemLevel() => await _dataProvider.GetMaxCraftedItemLevel();
        public async Task<ushort> GetItemLevel(ushort itemId) => await _dataProvider.GetItemLevel(itemId);

        /// <summary>
        /// Reload all <see cref="IGameDataProvider"/> objects. This will also clear provider caches.
        /// </summary>
        public void ReloadProvider()
        {
            try
            {
                _dataProvider = new LuminaProvider();
                FoundSqpack = true;
            }
            catch (DirectoryNotFoundException)
            {
                _dataProvider = new GithubRawProvider(_http);
                FoundSqpack = false;
            }
        }
    }
}
