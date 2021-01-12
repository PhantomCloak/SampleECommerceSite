using System.Threading.Tasks;
using EFurni.Contract.V1;
using EFurni.Contract.V1.Responses;
using EFurni.Presentation.Clients.ClientInterfaces;
using EFurni.Presentation.RestClientExtensions;
using EFurni.Shared.DTOs;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers.Newtonsoft.Json;
using RestRequest = RestSharp.RestRequest;

namespace EFurni.Presentation.Clients
{
    public class SummaryClient : ISummaryClient
    {
        private readonly RestClient _client;
        public SummaryClient(RestClientManager restClientManager)
        {
            _client = restClientManager.GetRestClient();
        }
        
        
        public async Task<ProductInfoDto> GetProductInfoAsync()
        {
            var productSummaryReq = new RestRequest(ApiRoutes.Summary.Get, Method.GET)
                .AddParameter("infoName", "productInfo", ParameterType.UrlSegment);

            var requestProductInfo = await _client.ExecuteAsync<Response<ProductInfoDto>>(productSummaryReq);

            var test = JsonConvert.DeserializeObject<Response<ProductInfoDto>>(requestProductInfo.Content);
            
            return requestProductInfo.Data.Data;
        }

        public async Task<BrandInfoDto> GetBrandInfoAsync()
        {
            var brandSummary = new RestRequest(ApiRoutes.Summary.Get, Method.GET)
                .AddParameter("infoName", "brandInfo", ParameterType.UrlSegment);
            
            var requestBrandInfo = await _client.ExecuteAsync<Response<BrandInfoDto>>(brandSummary);
            
            return requestBrandInfo.Data.Data;
        }

        public async Task<CategoryInfoDto> GetCategoryInfoAsync()
        {
            var categorySummary = new RestRequest(ApiRoutes.Summary.Get, Method.GET)
                .AddParameter("infoName", "categoryInfo", ParameterType.UrlSegment);
            
            var requestCategoryInfo = await _client.ExecuteAsync<Response<CategoryInfoDto>>(categorySummary);
            
            return requestCategoryInfo.Data.Data;
        }
    }
}