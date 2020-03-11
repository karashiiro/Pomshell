using Pomshell.Resources;
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

        /// <summary>
        /// Builds a char array of checked languages from a Lodestone language string.
        /// Returns an array ['j', 'e', 'f', 'd'], with capital characters where checked.
        /// </summary>
        public static string[] BuildLanguageArray(string languages)
        {
            string[] langsChecked = CompiledRegexes.ForwardSlashes.Split(languages);
            string[] output = { "JA", "EN", "FR", "DE" };
            for (byte i = 0; i < 4; i++)
                if (!output[i].Equals(langsChecked[i]))
                    output[i] = "none";
            return output;
        }

        public static string GetEndOfUriPath(string uri)
        {
            var things = new List<string>(CompiledRegexes.ForwardSlashes.Split(uri).Where(str => str != string.Empty));
            return things[things.Count() - 1];
        }

        public static char ToUpper(this char input)
            => char.ToUpper(input);
    }
}
