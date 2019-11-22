using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWS
{
    public static class Extensions
    {
        public static string TrimEnd(this string source, string value)
        {
            if (!source.EndsWith(value, StringComparison.InvariantCulture))
                return source;

            return source.Remove(source.LastIndexOf(value, StringComparison.InvariantCulture));
        }
    }
}
