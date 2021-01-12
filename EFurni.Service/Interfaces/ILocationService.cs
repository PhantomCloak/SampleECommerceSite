using System.Collections.Generic;
using System.Threading.Tasks;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Shared.DTOs;
using EFurni.Shared.Models;

namespace EFurni.Services
{
    public interface ILocationService
    {
        Task<IEnumerable<CountryDto>> GetCountriesAsync(PaginationParams paginationParams = null);
        Task<IEnumerable<ProvinceDto>> GetProvincesAsync(string countryTag,PaginationParams paginationParams = null);
        Task<IEnumerable<DistrictDto>> GetDistrictsAsync(string countryTag, string provinceName,PaginationParams paginationParams = null);
        Task<IEnumerable<NeighborhoodDto>> GetNeighborhoodsAsync(string countryTag, string provinceName,string districtName,PaginationParams paginationParams = null);
        Task<GenericAddress> GetAddressFromZip(string zipCode);
    }
}