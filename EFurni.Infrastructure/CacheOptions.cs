using System;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;

namespace EFurni.Infrastructure
{
    public class CacheOptions : ICacheOptions
    {
        public DistributedCacheEntryOptions FromConfiguration(IConfiguration configuration,string configRootName)
        {
            string slidingExpiration = configuration[$"{configRootName}:SlidingExpiration"];
            string relativeExpiration = configuration[$"{configRootName}:RelativeExpiration"];

            int.TryParse(slidingExpiration, out int slidingExpirationInMinutes);
            int.TryParse(slidingExpiration, out int relativeExpirationInMinutes);

            if (slidingExpirationInMinutes <= 0 && relativeExpirationInMinutes <= 0 )
            {
                return null;
            }
            
            var slidingExpirationTime = TimeSpan.FromMinutes(slidingExpirationInMinutes);
            var relativeExpirationTime = TimeSpan.FromMinutes(relativeExpirationInMinutes);
            
            return new DistributedCacheEntryOptions
            {
                SlidingExpiration = slidingExpirationTime, 
                AbsoluteExpirationRelativeToNow = relativeExpirationTime,
            };
        }
    }
}