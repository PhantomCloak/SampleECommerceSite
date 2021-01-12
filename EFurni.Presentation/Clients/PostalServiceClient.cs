using System.Collections.Generic;
using System.Net;
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
    public class PostalServiceClient : IPostalServiceClient
    {
        private readonly RestClient _client;
        public PostalServiceClient(RestClientManager clientManager)
        {
            _client = clientManager.GetRestClient();
        }

        public async Task<IEnumerable<PostalServiceDto>> GetPostalServices()
        {
            var basketDeleteRequest = new RestRequest(ApiRoutes.PostalService.GetAll, Method.GET);

            var response = await _client.ExecuteAsync<Response<IEnumerable<PostalServiceDto>>>(basketDeleteRequest);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            return response.Data.Data;
        }
    }
}