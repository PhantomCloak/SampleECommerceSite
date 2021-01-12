using System.Collections.Generic;

namespace EFurni.Presentation.Extensions
{
    public static class DictionaryExtensions
    {
        public static void Set<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if(!dict.ContainsKey(key))
            {
                dict[key] = value;
            }
            else
            {
                dict[key] = value;
            }
        }
    }
}