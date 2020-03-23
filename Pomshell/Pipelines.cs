using Newtonsoft.Json.Linq;
using Pomshell.Models;
using Pomshell.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pomshell
{
    public static class Pipelines
    {
        private static readonly string[] _gearPieces = { "Body", "Bracelets", "Earrings", "Feet", "Hands", "Legs", "MainHand", "Necklace", "SoulCrytal", "OffHand", "Ring1", "Ring2", "Waist" };

        public static async Task<long> GetCharacterLastActivityTime(GameDataService gameData, XIVAPIService xivapi, ulong id)
            => await GetCharacterLastActivityTime(gameData, xivapi, await xivapi.GetCharacter(id));

        public static async Task<long> GetCharacterLastActivityTime(GameDataService gameData, XIVAPIService xivapi, JObject fullCharacter)
        {
            long lastActivityTime = -1;

            if (fullCharacter["AchievementsPublic"].ToObject<bool>()) foreach (AchievementEntry achievement in fullCharacter["Achievements"]["List"].ToObject<IList<AchievementEntry>>())
            {
                if (achievement.Date * 1000 > lastActivityTime)
                {
                    lastActivityTime = achievement.Date * 1000;
                }
            }

            /*foreach (MinionMountEntry minion in fullCharacter["Minions"].Children().ToList() as IList<MinionMountEntry>)
            {
                // Check against minions by patch
            }

            foreach (MinionMountEntry mount in fullCharacter["Mounts"].Children().ToList() as IList<MinionMountEntry>)
            {
                // Check against mounts by patch
            }*/

            // Check gear release times
            List<GearItem> gearItems = new List<GearItem>();
            var gearItemsRaw = fullCharacter["Character"]["GearSet"]["Gear"];
            foreach (string piece in _gearPieces)
            {
                if (gearItemsRaw.SelectToken(piece) == null)
                    continue;
                gearItems.Add(gearItemsRaw[piece].ToObject<GearItem>());
            }
            var releaseDates = await xivapi.GetItemsReleaseDates(gearItems
                .Where(item => item.ID != null)
                .Select(item => (ushort)item.ID));
            foreach (KeyValuePair<ushort, long> result in releaseDates)
            {
                if (result.Value > lastActivityTime)
                {
                    lastActivityTime = result.Value;
                }
            }
            
            return await Task.FromResult(lastActivityTime);
        }
    }
}
