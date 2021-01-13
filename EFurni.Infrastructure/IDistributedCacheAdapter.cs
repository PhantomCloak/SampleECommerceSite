using System;
using System.Threading.Tasks;

namespace EFurni.Infrastructure
{
    public interface IDistributedCacheAdapter
    {
        Task SetString(string key,string value);
        Task SetString(string key,string value,TimeSpan ttl);
        Task SetAsync<T>(string key, T value);
        Task SetAsync<T>(string key, T value,TimeSpan ttl);
        Task SetInt(string key, int value);
        Task SetInt(string key, int value,TimeSpan ttl);
        Task DeleteAsync(string key);
        Task<T> GetAsync<T>(string key);
        Task<string> GetString(string key);
        Task<int> GetInt(string key);
    }
}