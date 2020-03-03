using System.Threading.Tasks;

namespace Pomshell.Services.GameData
{
    public interface IGameDataProvider
    {
        /// <summary>
        /// Gets the highest crafted battle gear item level.
        /// </summary>
        Task<ushort> GetMaxCraftedItemLevel();
    }
}
