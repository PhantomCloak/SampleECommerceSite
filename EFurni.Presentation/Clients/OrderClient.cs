using System;
using System.Net;
using System.Threading.Tasks;
using EFurni.Contract.V1;
using EFurni.Contract.V1.Queries;
using EFurni.Contract.V1.Responses;
using EFurni.Presentation.Clients.ClientInterfaces;
using EFurni.Shared.DTOs;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;

namespace EFurni.Presentation.Clients
{
    public class OrderClient : IOrderClient
    {
        private readonly IAuthenticationClient _authenticationClient;
        private readonly RestClient _client;
        
        public OrderClient(IAuthenticationClient authenticationClient, IConfiguration configuration)
        {
           _client = new RestClient(configuration["ApiServer:Address"]);
           _authenticationClient = authenticationClient;
        }
        
        public async Task<CustomerOrderDto> CreateOrder(CreateOrderQuery createOrderQuery)
        { 
            var createOrderReq = new RestRequest(ApiRoutes.Order.Create, Method.POST);
            var str = await _authenticationClient.AuthenticationToken();
            Console.WriteLine(str);
            // createOrderReq.AddParameter("Authorization",str,ParameterType.HttpHeader);
            createOrderReq.AddHeader("Authorization", str);
            
            createOrderReq.AddJsonBody(createOrderQuery);
            
            var response = await _client.ExecuteAsync(createOrderReq);

            if (response.StatusCode != HttpStatusCode.Created)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<Response<CustomerOrderDto>>(response.Content).Data;
        }

        public async Task<CustomerOrderDto> GetOrder(int orderId)
        {
            var getOrderReq = new RestRequest(ApiRoutes.Order.Get, Method.GET);
            getOrderReq.AddParameter("Authorization",await _authenticationClient.AuthenticationToken(),ParameterType.HttpHeader);
            getOrderReq.AddParameter("orderId", orderId, ParameterType.UrlSegment);
            
            var response = await _client.ExecuteAsync<Response<CustomerOrderDto>>(getOrderReq);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }
            
            var obj = JsonConvert.DeserializeObject<Response<CustomerOrderDto>>(response.Content);

            return obj.Data;
        }
    }
}