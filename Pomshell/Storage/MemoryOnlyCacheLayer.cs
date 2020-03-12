using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pomshell.Storage
{
    #pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public class MemoryOnlyCacheLayer : ICacheLayer
    {
        private readonly IDictionary<string, object> _backingService;

        public MemoryOnlyCacheLayer()
            => _backingService = new Dictionary<string, object>();

        public async Task SetItem<T>(string key, T obj) => _backingService.Add(key, obj);
        public async Task<T> GetItem<T>(string key) => (T)_backingService[key];
        public async Task RemoveItem(string key) => _backingService.Remove(key);
        public async Task<bool> Contains(string key) => _backingService.TryGetValue(key, out _);
        public async Task<int> Count() => await Task.FromResult(_backingService.Count);
        public async Task Clear() => await Task.Run(_backingService.Clear);
    }
    #pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
}