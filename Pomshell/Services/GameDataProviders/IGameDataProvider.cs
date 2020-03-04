using System.Threading.Tasks;

namespace Pomshell.Services.GameData
{
    public interface IGameDataProvider
    {
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
