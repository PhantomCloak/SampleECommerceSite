using System.Collections.Generic;
using System.Threading.Tasks;
using EFurni.Services;
using EFurni.Shared.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace EFurni.Service
{
    internal class CachedSummaryCache : ISummaryService
    {
        private readonly ISummaryService _summaryService;
        private readonly IMemoryCache _memoryCache;
        private readonly int _cacheExpirationInMinutes;

        public CachedSummaryCache(
            ISummaryService summaryService,
            IMemoryCache memoryCache,
            IConfiguration configuration)
        {
            _summaryService = summaryService;
            _memoryCache = memoryCache;
            _cacheExpirationInMinutes = int.Parse(configuration["SummaryService:LocalTokenExpireInMinutes"]);

        }
        
        public async Task<IEnumerable<Info>> GetAllInformationAsync()
        {
            return await _summaryService.GetAllInformationAsync();
        }

        public async Task<Info> GetInformationByNameAsync(string infoName)
        {
            return await _summaryService.GetInformationByNameAsync(infoName);
        }

        public async Task<Info[]> GenerateInfoAsync()
        {
            return await _summaryService.GenerateInfoAsync();
        }
    }
}