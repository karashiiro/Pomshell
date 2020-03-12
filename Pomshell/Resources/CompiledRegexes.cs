using System.Text.RegularExpressions;

namespace Pomshell.Resources
{
    public static class CompiledRegexes
    {
        public static Regex ForwardSlashes = new Regex(@"\/+", RegexOptions.Compiled);
        public static Regex NoDigits = new Regex(@"[^\d]", RegexOptions.Compiled);
        public static Regex Spaces = new Regex(@" +", RegexOptions.Compiled);
    }
}
