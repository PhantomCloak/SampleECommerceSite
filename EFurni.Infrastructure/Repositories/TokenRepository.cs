using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace EFurni.Infrastructure.Repositories
{
    internal class TokenRepository : ITokenRepository
    {
        private readonly IDistributedCacheAdapter _cacheAdapter;
        private readonly IConfiguration _configuration;

        public TokenRepository(
            IDistributedCacheAdapter cacheAdapter,
            IConfiguration configuration)
        {
            _configuration = configuration;
            _cacheAdapter = cacheAdapter;
        }
        
        public async Task<string> CreateTokenAsync(int actorId)
        {
            string token = Guid.NewGuid().ToString();
            var expiration = TimeSpan.FromMinutes(int.Parse(_configuration["TokenService:TokenExpireInMinutes"]));

            var cacheIdFromTokenKey = $"token:actorId:{token}";
            var cacheTokenFromIdKey = $"token:actorToken:{actorId}";

            await _cacheAdapter.SetInt(cacheIdFromTokenKey, actorId,expiration);
            await _cacheAdapter.SetString(cacheTokenFromIdKey, token,expiration);
            
            return token;
        }

        public async Task<string> TokenFromActorId(int actorId)
        {
            var cacheTokenFromIdKey = $"token:actorToken:{actorId}";
            var token = await _cacheAdapter.GetString(cacheTokenFromIdKey);
            
            return token;
        }

        public async Task<string> ActorIdFromToken(string token)
        { 
            var cacheIdFromTokenKey = $"token:actorId:{token}";
            var actorId = await _cacheAdapter.GetString(cacheIdFromTokenKey);

            return actorId;
        }

        public async Task<bool> DeleteTokenAsync(string token)
        {
            var actorId = ActorIdFromToken(token);
            
            var cacheIdFromTokenKey = $"token:actorId:{token}";
            var cacheTokenFromIdKey = $"token:actorToken:{actorId}";

            await _cacheAdapter.DeleteAsync(cacheIdFromTokenKey);
            await _cacheAdapter.DeleteAsync(cacheTokenFromIdKey);
            
            return true;
        }
    }
}