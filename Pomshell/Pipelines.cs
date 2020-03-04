﻿using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Pomshell.Models;
using Pomshell.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pomshell
{
    public static class Pipelines
    {
        public static async Task<long> GetCharacterLastActivityTime(ServiceProvider services, ulong id)
            => await GetCharacterLastActivityTime(services, await services.GetRequiredService<XIVAPIService>().GetCharacter(id));

        public static async Task<long> GetCharacterLastActivityTime(ServiceProvider services, JObject fullCharacter)
        {
            long lastActivityTime = -1;

            var gt = services.GetRequiredService<GarlandToolsService>();

            if ((bool)fullCharacter["AchievementsPublic"]) foreach (AchievementEntry achievement in fullCharacter["Achievements"]["List"].Children().ToList() as IList<AchievementEntry>)
                {
                    if (achievement.Date * 1000 > lastActivityTime)
                    {
                        lastActivityTime = achievement.Date * 1000;
                    }
                }

            foreach (MinionMountEntry minion in fullCharacter["Minions"].Children().ToList() as IList<MinionMountEntry>)
            {
                // Check against minions by patch
            }

            foreach (MinionMountEntry mount in fullCharacter["Mounts"].Children().ToList() as IList<MinionMountEntry>)
            {
                // Check against mounts by patch
            }

            foreach (GearItem item in fullCharacter["Character"]["GearSet"]["Gear"].Children().ToList() as IList<GearItem>)
            {
                if (item.Id == 0) // Check against gear by patch
                {
                    // lastActivityTime = time of patch release
                }
            }

            return await Task.FromResult(lastActivityTime);
        }
    }
}