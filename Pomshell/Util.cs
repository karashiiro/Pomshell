using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Pomshell
{
    public static class Util
    {
        public static string ProgramFilesx86()
        {
            if (Environment.Is64BitOperatingSystem)
            {
                return Environment.GetEnvironmentVariable("ProgramFiles(x86)") ?? @"C:\Program Files (x86)";
            }

            return Environment.GetEnvironmentVariable("ProgramFiles") ?? @"C:\Program Files";
        }

        public static string BuildLodestoneUrl(ulong id)
            => $"https://na.finalfantasyxiv.com/lodestone/character/{id}/";

        public static string[] BuildLanguageArray(string langs)
        {
            string[] lang = Regex.Split(langs, @"\/+");
            string[] output = { lang[0] ?? "", lang[1] ?? "", lang[2] ?? "", lang[3] ?? "" };
            return output;
        }

        public static string GetEndOfUriPath(string uri)
        {
            var things = new List<string>(Regex.Split(uri, @"\/").Where(str => str != string.Empty));
            return things[things.Count() - 1];
        }
    }
}
