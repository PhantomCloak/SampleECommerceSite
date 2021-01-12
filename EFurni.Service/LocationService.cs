using System.Collections.Generic;
using System.Threading.Tasks;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Infrastructure.Queries;
using EFurni.Shared.DTOs;
using EFurni.Shared.Models;

namespace EFurni.Services
{
    internal class LocationService : ILocationService
    {
        private readonly ILocationQuery _locationQuery;
        public LocationService(ILocationQuery locationQuery)
        {
            _locationQuery = locationQuery;
        }

        public async Task<IEnumerable<CountryDto>> GetCountriesAsync(PaginationParams paginationParams = null)
        {
            var countries = await _locationQuery.GetCountriesAsync(paginationParams);

            return countries;
        }

        public async Task<IEnumerable<ProvinceDto>> GetProvincesAsync(string countryTag,PaginationParams paginationParams = null)
        {
            var provinces = await _locationQuery.GetProvincesAsync(countryTag, paginationParams);

            return provinces;
        }

        public async Task<IEnumerable<DistrictDto>> GetDistrictsAsync(string countryTag, string provinceName,PaginationParams paginationParams = null)
        {
            var districts = await _locationQuery.GetDistrictsAsync(countryTag,provinceName, paginationParams);

            return districts;
        }

        public async Task<IEnumerable<NeighborhoodDto>> GetNeighborhoodsAsync(string countryTag, string provinceName,string districtName,PaginationParams paginationParams = null)
        {
            var districts = await _locationQuery.GetNeighborhoodsAsync(countryTag,provinceName,districtName, paginationParams);

            return districts;
        }

        public async Task<GenericAddress> GetAddressFromZip(string zipCode)
        {
            var address = await _locationQuery.GetAddressFromZipAsync(zipCode);

            return address;
        }
    }
}