using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mc2.CrudTest.Presentation.Shared.Helper
{
    public static partial class RegexPatterns
    {
        public static readonly Regex EmailIsValid = EmailRegexPatternAttr();

        [GeneratedRegex(
            @"^([0-9a-zA-Z]([+\-_.][0-9a-zA-Z]+)*)+@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,17})$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant)]
        private static partial Regex EmailRegexPatternAttr();
    }
}
