using Blazored.LocalStorage;
using Pomshell.Storage;
using System.Threading.Tasks;

namespace Pomshell.Blazor.Client.Storage
{
    public class LocalStorageCacheLayer : ICacheLayer
    {
        private readonly ILocalStorageService _backingService;

        public LocalStorageCacheLayer(ILocalStorageService backingService)
            => _backingService = backingService;

        public async Task SetItem<T>(string key, T obj) => await _backingService.SetItemAsync(key, obj);
        public async Task<T> GetItem<T>(string key) => await _backingService.GetItemAsync<T>(key);
        public async Task RemoveItem(string key) => await _backingService.RemoveItemAsync(key);
        public async Task<bool> Contains(string key) => await _backingService.ContainKeyAsync(key);
        public async Task<int> Count() => await _backingService.LengthAsync();
        public async Task Clear() => await _backingService.ClearAsync();
    }
}
