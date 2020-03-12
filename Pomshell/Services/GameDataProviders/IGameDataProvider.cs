using Pomshell.Storage;
using System.Threading.Tasks;

namespace Pomshell.Services.GameData
{
    public interface IGameDataProvider
    {
        /// <summary>
        /// Creates a cache on the provider, deleting any existing cache.
        /// </summary>
        void RebuildCache<T>() where T : ICacheLayer;

        /// <summary>
        /// Gets the highest crafted battle gear item level.
        /// </summary>
        Task<ushort> GetMaxCraftedItemLevel();

        /// <summary>
        /// Gets the item level of an item.
        /// </summary>
        Task<ushort> GetItemLevel(ushort itemId);
    }
}
