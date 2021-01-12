using System.Collections.Generic;
using System.Threading.Tasks;
using EFurni.Contract.V1;
using EFurni.Contract.V1.Queries.Filter;
using EFurni.Contract.V1.Responses;
using EFurni.Presentation.Clients.ClientInterfaces;
using EFurni.Presentation.RestClientExtensions;
using EFurni.Shared.DTOs;
using EFurni.Shared.Models;
using EFurni.Shared.Types;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace EFurni.Presentation.Clients
{
    public class StoreClient : IStoreClient
    {
        private readonly RestClient _client;
        
        public StoreClient(RestClientManager restClientManager)
        {
            _client = restClientManager.GetRestClient();
        }

        public async Task<IEnumerable<MatchTuple<EntityDistanceInfo<StoreDto>, int>>> GetNearestStores(string zipCode, int[] productIds)
        {
            var nearStoreReq = new RestRequest(ApiRoutes.StoreAlias.StoreMatch, Method.POST);

            var aa = new StoreMatchQuery
            {
                NearestStoreFromZip = zipCode,
                ProductsInStock = productIds
            };
            nearStoreReq.AddJsonBody(aa);
            
            var response = await _client.ExecuteAsync<Response<IEnumerable<MatchTuple<EntityDistanceInfo<StoreDto>, int>>>>(nearStoreReq);

                return response.Data.Data;
        }
    }
}