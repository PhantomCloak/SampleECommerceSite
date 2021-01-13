using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace EFurni.Infrastructure
{
    public class DistributedCacheAdapter : IDistributedCacheAdapter
    {
        private IConnectionMultiplexer _redis;
        private static TimeSpan _defaultTtl = TimeSpan.Zero;
        
        public DistributedCacheAdapter(IConnectionMultiplexer redis,
            IConfiguration configuration)
        {
            _redis = redis;

            //potential update issue
            if (_defaultTtl == TimeSpan.Zero)
            {
                var ttl = int.Parse(configuration["Redis:DefaultTTL"]);
                _defaultTtl = TimeSpan.FromMinutes(ttl);
            }
        }
        
        public async Task SetString(string key, string value)
        {
            await SetString(key, value, _defaultTtl);
        }

        public async Task SetString(string key, string value, TimeSpan ttl)
        {
            var db = _redis.GetDatabase();
            await db.StringSetAsync(key, value, ttl);
        }

        public async Task SetAsync<T>(string key, T value)
        {
            await SetAsync(key, value, _defaultTtl);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan ttl)
        {
            var db = _redis.GetDatabase();
            var objJson = JsonConvert.SerializeObject(value,new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            
            await db.StringSetAsync(key, objJson, ttl);
        }

        public async Task SetInt(string key, int value)
        {
            await SetInt(key, value, _defaultTtl);
        }

        public async Task SetInt(string key, int value, TimeSpan ttl)
        {
            var db = _redis.GetDatabase();
            await db.StringSetAsync(key, value, ttl);
        }

        public async Task DeleteAsync(string key)
        {
            var db = _redis.GetDatabase();
            await db.KeyDeleteAsync(key);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var db = _redis.GetDatabase();
            var objJson = await db.StringGetAsync(key);

            if (string.IsNullOrEmpty(objJson))
                return default;
            
            return JsonConvert.DeserializeObject<T>(objJson);
        }

        public async Task<string> GetString(string key)
        {
            var db = _redis.GetDatabase();
            return await db.StringGetAsync(key);
        }

        public async Task<int> GetInt(string key)
        {
            var db = _redis.GetDatabase();
            var value = await db.StringGetAsync(key);
            return int.Parse(value);
        }
    }
}