using System.Collections.Generic;

namespace Common.Utility
{
    public static class DictionaryExtensions
    {
        public static void AddOrCreate<TKey>(this IDictionary<TKey, double> dict, TKey key, double val)
        {
            if (!dict.ContainsKey(key))
            {
                dict.Add(key, val);
            }
            else
            {
                dict[key] += val;
            }
        }

        public static void AddOrCreate<TKey>(this IDictionary<TKey, int> dict, TKey key, int val)
        {
            if (!dict.ContainsKey(key))
            {
                dict.Add(key, val);
            }
            else
            {
                dict[key] += val;
            }
        }

        public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue val)
        {

            if (!dict.TryGetValue(key, out val))
            {
                dict.Add(key, val);
            }

            return val;
        }


    }
}
