using System.Collections.Generic;

namespace DarkSky.Messaging.Extensions {
    public static class DictionaryExtensions {
        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) {
            return dictionary.ContainsKey(key) ? dictionary[key] : default(TValue);
        }

        public static TValue GetValue<TValue>(this IDictionary<string, TValue> dictionary, string key) {
            return GetValue<string, TValue>(dictionary, key);
        }

        public static string GetValue(this IDictionary<string, string> dictionary, string key) {
            return GetValue<string, string>(dictionary, key);
        }
    }
}