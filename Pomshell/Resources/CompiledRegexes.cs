using System.Text.RegularExpressions;

namespace Pomshell.Resources
{
    public static class CompiledRegexes
    {
        public static Regex ForwardSlashes = new Regex(@"\/+", RegexOptions.Compiled);
    }
}
