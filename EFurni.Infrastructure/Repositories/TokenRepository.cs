using System;
using System.Threading.Tasks;
using EFurni.Infrastructure.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;

namespace EFurni.Infrastructure.Repositories
{
    internal class TokenRepository : ITokenRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IConfiguration _configuration;

        public TokenRepository(
            IDistributedCache distributedCache,
            IConfiguration configuration)
        {
            _configuration = configuration;
            _distributedCache = distributedCache;
        }
        
        public async Task<string> CreateTokenAsync(int actorId)
        {
            string token = Guid.NewGuid().ToString();
            var expiration = TimeSpan.FromMinutes(int.Parse(_configuration["TokenService:TokenExpireInMinutes"]));

            var cacheIdFromTokenKey = $"token:actorId:{token}";
            var cacheTokenFromIdKey = $"token:actorToken:{actorId}";

            var cacheOptions = _distributedCache
                .CacheOptions()
                .FromConfiguration(_configuration, "TokenCache");
                
            await _distributedCache.SetIntAsync(cacheIdFromTokenKey, actorId,cacheOptions);
            await _distributedCache.SetStringAsync(cacheTokenFromIdKey, token,cacheOptions);
            
            return token;
        }

        public async Task<string> TokenFromActorId(int actorId)
        {
            var cacheTokenFromIdKey = $"token:actorToken:{actorId}";
            var token = await _distributedCache.GetStringAsync(cacheTokenFromIdKey);
            
            return token;
        }

        public async Task<string> ActorIdFromToken(string token)
        { 
            var cacheIdFromTokenKey = $"token:actorId:{token}";
            var actorId = await _distributedCache.GetStringAsync(cacheIdFromTokenKey);

            return actorId;
        }

        public async Task<bool> DeleteTokenAsync(string token)
        {
            var actorId = ActorIdFromToken(token);
            
            var cacheIdFromTokenKey = $"token:actorId:{token}";
            var cacheTokenFromIdKey = $"token:actorToken:{actorId}";

            await _distributedCache.RemoveAsync(cacheIdFromTokenKey);
            await _distributedCache.RemoveAsync(cacheTokenFromIdKey);
            
            return true;
        }
    }
}