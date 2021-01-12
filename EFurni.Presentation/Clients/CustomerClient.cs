using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EFurni.Contract.V1;
using EFurni.Contract.V1.Responses;
using EFurni.Presentation.Clients.ClientInterfaces;
using EFurni.Presentation.RestClientExtensions;
using EFurni.Shared.DTOs;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using RestSharp;
#pragma warning disable 8603

namespace EFurni.Presentation.Clients
{
    public class CustomerClient : ICustomerClient
    {
        private readonly RestClient _client;
        private readonly IAuthenticationClient _authenticationClient;
        
        public CustomerClient(RestClientManager restClientManager, IAuthenticationClient authenticationClient)
        {
            _authenticationClient = authenticationClient;
            _client = restClientManager.GetRestClient();
        }
        
        public async Task<CustomerDto> GetCustomer(int customerId)
        {
            var customerRequest = new RestRequest(ApiRoutes.Customer.Get, Method.GET);
            
            customerRequest.AddParameter("Authorization",await _authenticationClient.AuthenticationToken(),ParameterType.HttpHeader);
            customerRequest.AddParameter("customerId", customerId, ParameterType.UrlSegment);

            var response = await _client.ExecuteAsync<Response<CustomerDto>>(customerRequest);

            if (response.Data == null)
            {
                return null;
            }
            
            return response.Data.Data;
        }

        public async Task<CustomerDto> GetSelfAsync()
        {
            var customerRequest = new RestRequest(ApiRoutes.CustomerAlias.GetSelf, Method.GET);

            string selfToken = await _authenticationClient.AuthenticationToken();

            if (string.IsNullOrEmpty(selfToken))
                return null;
            
            customerRequest.AddParameter("Authorization",selfToken,ParameterType.HttpHeader);

            var response = await _client.ExecuteAsync<Response<CustomerDto>>(customerRequest);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }
            
            return response.Data.Data;
        }
    }
}