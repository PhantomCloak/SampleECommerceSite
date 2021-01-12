using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EFurni.Contract.V1;
using EFurni.Contract.V1.Responses;
using EFurni.Presentation.Clients.ClientInterfaces;
using EFurni.Shared.Models;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace EFurni.Presentation.Clients
{
    public class BasketClient : IBasketClient
    {
        private IAuthenticationClient _authenticationClient;
        private RestClient _client;
        
        public BasketClient(IConfiguration configuration, IAuthenticationClient authenticationClient)
        {
            _authenticationClient = authenticationClient;
            _client = new RestClient(configuration["ApiServer:Address"]);
        }

        public async Task<IEnumerable<BasketItem>> GetAllBasketProducts()
        {
            var productReq = new RestRequest(ApiRoutes.BasketItems.GetAll, Method.GET)
                .AddHeader("Authorization",await _authenticationClient.AuthenticationToken());

            var response = await _client.ExecuteAsync<Response<IEnumerable<BasketItem>>>(productReq);

            if (response.Data == null)
            {
                return Enumerable.Empty<BasketItem>();
            }
            
            return response.Data.Data;
        }

        public async Task<bool> CreateBasketProductAsync(int productId, int amount)
        {
            var productReq = new RestRequest(ApiRoutes.BasketItems.Create, Method.POST)
                .AddHeader("Authorization", await _authenticationClient.AuthenticationToken())
                .AddParameter("productId",productId)
                .AddParameter("amount",amount);

            var response = await _client.ExecuteAsync(productReq);
            
            return response.StatusCode == HttpStatusCode.OK;
        }

        public async Task<BasketItem> GetBasketProduct(int productId)
        {
            var basketGetRequest = new RestRequest(ApiRoutes.BasketItems.Get, Method.GET)
                .AddHeader("Authorization", await _authenticationClient.AuthenticationToken())
                .AddParameter("productId", productId,ParameterType.UrlSegment);

           var response = await _client.ExecuteAsync<BasketItem>(basketGetRequest);

           return response.Data;
        }

        public async Task<bool> RemoveBasketProduct(int productId)
        {
            var basketDeleteRequest = new RestRequest(ApiRoutes.BasketItems.Delete, Method.DELETE)
                .AddHeader("Authorization",await _authenticationClient.AuthenticationToken())
                .AddParameter("productId", productId,ParameterType.UrlSegment);

            var response = await _client.ExecuteAsync<BasketItem>(basketDeleteRequest);

            return response.StatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> UpdateBasketProduct(int productId,int count)
        {
            var basketUpdateRequest = new RestRequest(ApiRoutes.BasketItems.Update, Method.DELETE)
                .AddHeader("Authorization", await _authenticationClient.AuthenticationToken())
                .AddParameter("productId", productId,ParameterType.UrlSegment)
                .AddParameter("count", count);

            var response = await _client.ExecuteAsync<BasketItem>(basketUpdateRequest);

            return response.StatusCode == HttpStatusCode.OK;
        }

        public Task<bool> UpdateOrAddInc(int productId)
        {
            throw new System.NotImplementedException();
        }

        public async Task ClearAllBasket()
        {
            throw new System.NotImplementedException();
        }
    }
}