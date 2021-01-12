using System.Collections.Generic;
using System.Threading.Tasks;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Shared.DTOs;
using EFurni.Shared.Models;

namespace EFurni.Infrastructure.Queries
{
    public interface ILocationQuery
    {
        public Task<IEnumerable<CountryDto>> GetCountriesAsync(PaginationParams paginationParams = null);
        public Task<IEnumerable<ProvinceDto>> GetProvincesAsync(string countyTag, PaginationParams paginationParams = null);
        public Task<IEnumerable<DistrictDto>> GetDistrictsAsync(string countryTag, string provinceName, PaginationParams paginationParams = null);
        public Task<IEnumerable<NeighborhoodDto>> GetNeighborhoodsAsync(string countryTag, string provinceName, string districtName, PaginationParams paginationParams = null);
        public Task<(decimal latitude, decimal longitude)> GetZipCodeCoordinateAsync(string zipCode);
        public Task<IEnumerable<ZipCodeLocationPair>> GetZipCodeDistancesAsync(string sourcePostalCode,IEnumerable<string> destinationPostalCodes);
        public Task<GenericAddress> GetAddressFromZipAsync(string zipCode);
    }
}