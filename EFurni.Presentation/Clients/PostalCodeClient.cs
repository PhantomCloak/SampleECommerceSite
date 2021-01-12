using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFurni.Contract.V1;
using EFurni.Contract.V1.Responses;
using EFurni.Presentation.Clients.ClientInterfaces;
using EFurni.Presentation.RestClientExtensions;
using EFurni.Shared.DTOs;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace EFurni.Presentation.Clients
{
    public class PostalCodeClient : IPostalCodeClient
    {

        private readonly RestClient _client;
        
        public PostalCodeClient(RestClientManager clientManager)
        {
            _client = clientManager.GetRestClient();
        }
        
        public async Task<IEnumerable<CountryDto>> GetCountries()
        {
            var locationRequest = new RestRequest(ApiRoutes.Location.GetCountries, Method.GET);

            var response = await _client.ExecuteAsync<Response<IEnumerable<CountryDto>>>(locationRequest);

            if (response.Data == null)
            {
                return Enumerable.Empty<CountryDto>();
            }
            
            return response.Data.Data;
        }

        public async Task<IEnumerable<ProvinceDto>> GetProvinces(string countryTag)
        {
            var locationRequest = new RestRequest(ApiRoutes.Location.GetProvince, Method.GET);

            locationRequest.AddParameter("countryTag", countryTag, ParameterType.UrlSegment);
            
            var response = await _client.ExecuteAsync<Response<IEnumerable<ProvinceDto>>>(locationRequest);

            if (response.Data == null)
            {
                return Enumerable.Empty<ProvinceDto>();
            }
            
            return response.Data.Data;
        }

        public async Task<IEnumerable<DistrictDto>> GetDistricts(string countryTag, string provinceName)
        {
            var locationRequest = new RestRequest(ApiRoutes.Location.GetDistrict, Method.GET);

            locationRequest.AddParameter("countryTag", countryTag, ParameterType.UrlSegment);
            locationRequest.AddParameter("provinceName", provinceName, ParameterType.UrlSegment);
                
            var response = await _client.ExecuteAsync<Response<IEnumerable<DistrictDto>>>(locationRequest);

            if (response.Data == null)
            {
                return Enumerable.Empty<DistrictDto>();
            }
            
            return response.Data.Data;
        }

        public async Task<IEnumerable<NeighborhoodDto>> GetNeighborhoods(string countryTag, string provinceName, string districtName)
        {
            var locationRequest = new RestRequest(ApiRoutes.Location.GetNeighborhood, Method.GET);

            locationRequest.AddParameter("countryTag", countryTag, ParameterType.UrlSegment);
            locationRequest.AddParameter("provinceName", provinceName, ParameterType.UrlSegment);
            locationRequest.AddParameter("districtName", districtName, ParameterType.UrlSegment);
                
            var response = await _client.ExecuteAsync<Response<IEnumerable<NeighborhoodDto>>>(locationRequest);

            if (response.Data == null)
            {
                return Enumerable.Empty<NeighborhoodDto>();
            }
            
            return response.Data.Data;
        }
    }
}