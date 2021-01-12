using System.Collections.Generic;
using System.Threading.Tasks;
using EFurni.Shared.DTOs;

namespace EFurni.Presentation.Clients.ClientInterfaces
{
    public interface IPostalCodeClient
    {
        public Task<IEnumerable<CountryDto>> GetCountries();
        public Task<IEnumerable<ProvinceDto>> GetProvinces(string countryTag);
        public Task<IEnumerable<DistrictDto>> GetDistricts(string countryTag,string provinceName);
        public Task<IEnumerable<NeighborhoodDto>> GetNeighborhoods(string countryTag,string provinceName,string districtName);
    }
}