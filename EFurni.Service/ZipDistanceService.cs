
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFurni.Infrastructure.Queries;
using EFurni.Shared.Models;

namespace EFurni.Services
{
    internal class ZipDistanceService : IZipDistanceService
    {
        private readonly ILocationQuery _locationQuery;

        public ZipDistanceService(ILocationQuery locationQuery)
        {
            _locationQuery = locationQuery;
        }

        public async Task<IEnumerable<ZipCodeLocationPair>> GetNearestZipCodes(string originZipCode, string[] destZipCodes)
        {
            var distances =await _locationQuery.GetZipCodeDistancesAsync(originZipCode,destZipCodes);

            return distances.ToArray();
        }
    }
}