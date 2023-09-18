using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE.Extensions.Extensions
{
    public static class DictionaryExtensions
    {
        public static TValue? GetValueOrNull<TKey, TValue>(
            this IDictionary<TKey, TValue> self,
            TKey key)
            where TValue : class
        {
            if (key != null //return null if key == null
                && self.TryGetValue(key, out var value))
            {
                return value;
            }
            
            return null;
        }

        public static TValue GetValueOrAdd<TKey, TValue>(
            this IDictionary<TKey, TValue> self,
            TKey key,
            Func<TValue> newValueSupplier)
            where TKey: notnull
        {
            if (self is ConcurrentDictionary<TKey, TValue> concurrentSelf)
            {
                return concurrentSelf.GetOrAdd(key, k => newValueSupplier());
            }
            else
            {
                if (!self.TryGetValue(key, out TValue value))
                {
                    value = newValueSupplier();
                    self.Add(key, value);
                }

                return value;
            }
        }
    }
}
