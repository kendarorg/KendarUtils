using Ku.Main.Commons;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Ku.Commons
{
    public class CacheService : ICacheService
    {
        public CacheService(int cacheDurationMinutes)
        {
            _cacheDurationInMinutes = cacheDurationMinutes;
        }

        private static string CalculateMD5Hash(string input)
        {
            if (input.Length <= 32) return input;
            // step 1, calculate MD5 hash from input
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();

        }

        private static Type GetGenericEnumerable(Type t)
        {
            var genArgs = t.GetGenericArguments();
            if (genArgs.Length == 1 && typeof(IEnumerable<>).MakeGenericType(genArgs).IsAssignableFrom(t))
            {
                return genArgs[0];
            }
            else if (t.BaseType != null)
            {
                return GetGenericEnumerable(t.BaseType);
            }
            return t;
        }

        private static ConcurrentDictionary<string, MemoryCache> _regions = new ConcurrentDictionary<string, MemoryCache>(StringComparer.InvariantCultureIgnoreCase);
        private int _cacheDurationInMinutes;

        private object GetCachedData(string region, string key, Func<object> valueFactory)
        {
            if (_cacheDurationInMinutes <= 0)
            {
                return valueFactory();
            }
            var cache = _regions.GetOrAdd(region, (t) => new MemoryCache(t));

            var newValue = new Lazy<object>(valueFactory);
            var value = (Lazy<object>)cache.AddOrGetExisting(key, newValue, new
                CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow + TimeSpan.FromMinutes(_cacheDurationInMinutes)
            });
            return (value ?? newValue).Value; // Lazy<T> handles the locking itself
        }

        private T GetCachedData<T>(string region, string key, Func<T> valueFactory)
        {
            if (_cacheDurationInMinutes <= 0)
            {
                return (T)valueFactory();
            }
            var cache = _regions.GetOrAdd(region, (t) => new MemoryCache(t));

            var newValue = new Lazy<T>(valueFactory);
            var value = (Lazy<T>)cache.AddOrGetExisting(key, newValue, new
                CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.ToOffset(TimeSpan.FromMinutes(_cacheDurationInMinutes))
            });
            return (value ?? newValue).Value; // Lazy<T> handles the locking itself
        }

        public void Invalidate(String region = "")
        {
            if (_cacheDurationInMinutes <= 0) return;
            if (region == "*")
            {
                _regions.Clear();
                return;
            }
            _regions.AddOrUpdate(region, (t) => new MemoryCache(region), (t, oldMem) =>
            {
                oldMem.Dispose();
                return new MemoryCache(region);
            });
        }

        public T GetOrAdd<T>(String key, Func<T> retrieveData, string region = "")
        {
            if (_cacheDurationInMinutes <= 0)
            {
                return retrieveData();
            }
            var hashKey = CalculateMD5Hash(key);
            return GetCachedData<T>(region, hashKey, () => retrieveData());
        }

        public void Remove(string key, string region = "")
        {
            if (_cacheDurationInMinutes <= 0) return;
            var hashedKey = CalculateMD5Hash(key);
            var cache = _regions.GetOrAdd(region, (t) => new MemoryCache(t));
            cache.Remove(hashedKey);
        }
    }
}
