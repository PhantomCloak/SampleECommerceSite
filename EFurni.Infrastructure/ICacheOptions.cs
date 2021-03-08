using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;

namespace EFurni.Infrastructure
{
    public interface ICacheOptions
    {
        DistributedCacheEntryOptions FromConfiguration(IConfiguration configuration, string configRootName);
    }
}