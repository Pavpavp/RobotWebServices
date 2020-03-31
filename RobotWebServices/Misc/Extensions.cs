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

        public static void AddOrOverwrite<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (dict.ContainsKey(key))
                dict[key] = value;
            else
                dict.Add(key, value);
        }
    }
}
