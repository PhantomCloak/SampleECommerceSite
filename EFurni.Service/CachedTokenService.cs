using System;
using System.Threading.Tasks;
using EFurni.Services;
using EFurni.Shared.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace EFurni.Service
{
    internal class CachedTokenService : ITokenService
    {
        private readonly ITokenService _tokenService;
        private readonly IMemoryCache _memoryCache;
        private readonly int _cacheExpirationInMinutes;
        
        public CachedTokenService(
            ITokenService tokenService,
            IMemoryCache memoryCache,
            IConfiguration configuration)
        {
            _tokenService = tokenService;
            _memoryCache = memoryCache;
            _cacheExpirationInMinutes = int.Parse(configuration["SummaryService:LocalTokenExpireInMinutes"]);
        }

        public async Task<string> GetAccountTokenAsync(Account account)
        {
            string cachedToken = _memoryCache.Get<string>(account);

            if (!string.IsNullOrEmpty(cachedToken))
            {
                return cachedToken;
            }
            
            var result =  await _tokenService.GetAccountTokenAsync(account);

            _memoryCache.Set(account, result, TimeSpan.FromMinutes(_cacheExpirationInMinutes));
            
            return result;
        }

        public async Task<Account> GetTokenAccountAsync(string token)
        {
            var cachedAccount = _memoryCache.Get<Account>(token);

            if (cachedAccount != null)
            {
                return cachedAccount;
            }
            
            var result = await _tokenService.GetTokenAccountAsync(token);

            _memoryCache.Set(token, result, TimeSpan.FromMinutes(_cacheExpirationInMinutes));
            
            return result;
        }

        public async Task<string> CreateTokenAsync(Account account)
        {
            string token = await _tokenService.CreateTokenAsync(account);

            _memoryCache.Set(token, account, TimeSpan.FromMinutes(_cacheExpirationInMinutes));
            _memoryCache.Set(account, token, TimeSpan.FromMinutes(_cacheExpirationInMinutes));
            
            return token;
        }

        public async Task DeleteTokenAsync(string token)
        { 
            await _tokenService.DeleteTokenAsync(token);
            _memoryCache.Remove(token);
        }
    }
}
