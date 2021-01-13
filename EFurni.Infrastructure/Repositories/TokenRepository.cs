using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace EFurni.Infrastructure.Repositories
{
    internal class TokenRepository : ITokenRepository
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IConfiguration _configuration;

        public TokenRepository(IConnectionMultiplexer redis, IConfiguration configuration)
        {
            _redis = redis;
            _configuration = configuration;
        }
        
        public async Task<string> CreateTokenAsync(int issuedIdentifier)
        {
            var db = _redis.GetDatabase();

            string token = Guid.NewGuid().ToString();
            var identifier = issuedIdentifier.ToString();
            
            var expiration = TimeSpan.FromMinutes(double.Parse(_configuration["TokenService:TokenExpireInMinutes"]));

            await db.HashSetAsync(ToServiceString(token),
                new[]
                {
                    new HashEntry("identifier",issuedIdentifier), 
                });

            await db.HashSetAsync(ToServiceString(identifier),
                new[]
                {
                    new HashEntry("token",token), 
                });

            db.KeyExpire(ToServiceString(token),expiration);
            db.KeyExpire(ToServiceString(identifier),expiration);
            
            return token;
        }

        public async Task<string> TokenFromActorId(int identifier)
        {
            var db = _redis.GetDatabase();

            var key = ToServiceString(identifier.ToString());
            
            var id = (string)(await db.HashGetAsync(key,"token"));

            return id;
        }

        public async Task<string> ActorIdFromToken(string token)
        { 
            var db = _redis.GetDatabase();

            var key = ToServiceString(token);
            
            var id = (string)(await db.HashGetAsync(key,"identifier"));

            return id;
        }

        public async Task<bool> DeleteTokenAsync(string token)
        {
            var db = _redis.GetDatabase();
            var identifier = await ActorIdFromToken(token);

            string key1 = ToServiceString(token);
            string key2 = ToServiceString(identifier.ToString());

            db.KeyDelete(key1);
            db.KeyDelete(key2);

            return true;
        }

        
        private string ToServiceString(string str)
        {
            return "token-service:" + str;
        }
    }
}