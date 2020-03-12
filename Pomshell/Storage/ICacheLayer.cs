using System.Threading.Tasks;

namespace Pomshell.Storage
{
    public interface ICacheLayer
    {
        /// <summary>
        /// Add an item to the cache.
        /// </summary>
        Task SetItem<T>(string key, T obj);

        /// <summary>
        /// Gets an item from the cache.
        /// </summary>
        Task<T> GetItem<T>(string key);

        /// <summary>
        /// Removes an item from the cache.
        /// </summary>
        Task RemoveItem(string key);

        /// <summary>
        /// Returns whether or not a key exists in the cache.
        /// </summary>
        Task<bool> Contains(string key);

        /// <summary>
        /// Retrieves the number of objects in the cache.
        /// </summary>
        Task<int> Count();

        /// <summary>
        /// Clear the entire cache.
        /// </summary>
        Task Clear();
    }
}
