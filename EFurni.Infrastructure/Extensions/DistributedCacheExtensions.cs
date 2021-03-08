using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace EFurni.Infrastructure.Extensions
{
    public static class DistributedCacheExtensions
    {
        public static async Task SetIntAsync(this IDistributedCache instance, string key,int value,DistributedCacheEntryOptions options = null)
        {
            await instance.SetStringAsync(key, value.ToString());
        }
        public static async Task<int?> GetIntAsync(this IDistributedCache instance,string key)
        {
            var payload = await instance.GetAsync(key);

            if (payload == null || payload.Length != 4)
            {
                return null;
            }

            return BitConverter.ToInt32(payload);
        }

        public static async Task<T> GetAsync<T>(this IDistributedCache instance, string key)
        {
            var objJson = await instance.GetStringAsync(key);

            if (string.IsNullOrEmpty(objJson))
                return default;
            
            return JsonConvert.DeserializeObject<T>(objJson);
        }

        public static async Task SetAsync<T>(this IDistributedCache instance, string key,T value, DistributedCacheEntryOptions options = null)
        {
            var objJson = JsonConvert.SerializeObject(value, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            await instance.SetStringAsync(key, objJson, options);
        }

        public static ICacheOptions CacheOptions(this IDistributedCache instance)
        {
            return new CacheOptions();
        }
    }
}