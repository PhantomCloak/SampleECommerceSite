using System.Collections.Generic;
using System.Threading.Tasks;
using EFurni.Contract.V1;
using EFurni.Contract.V1.Queries.Create;
using EFurni.Contract.V1.Responses;
using EFurni.Presentation.Clients.ClientInterfaces;
using EFurni.Shared.DTOs;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace EFurni.Presentation.Clients
{
    public class ReviewClient : IReviewClient
    {
        private readonly RestClient _client;
        private IAuthenticationClient _authenticationClient;

        
        public ReviewClient(IConfiguration configuration, IAuthenticationClient authenticationClient)
        {
            _authenticationClient = authenticationClient;
            _client = new RestClient(configuration["ApiServer:Address"]);
            _client.AddHandler("application/json", () => new RestSharp.Serializers.Newtonsoft.Json.NewtonsoftJsonSerializer());
        }
        
        public async Task<IEnumerable<CustomerReviewDto>> GetReviews(int productId)
        {
            var reviewRequest = new RestRequest(ApiRoutes.ProductReview.GetAll, Method.GET);
            reviewRequest.AddParameter("productId", productId, ParameterType.UrlSegment);
         
            var result = await _client.ExecuteAsync<Response<IEnumerable<CustomerReviewDto>>>(reviewRequest);
            
            return result.Data.Data;
        }

        public async Task<bool> CreateReviewAsync(int productId, string desc,int rating)
        {
            var reviewRequest = new RestRequest(ApiRoutes.ProductReview.Create, Method.POST);
            reviewRequest.AddParameter("Authorization",await _authenticationClient.AuthenticationToken(),ParameterType.HttpHeader);
            reviewRequest.AddParameter("productId", productId,ParameterType.UrlSegment);
            
            reviewRequest.AddParameter("ReviewText", desc, ParameterType.QueryString);
            reviewRequest.AddParameter("Rating",rating, ParameterType.QueryString);

            
            var result = await _client.ExecuteAsync<bool>(reviewRequest);

            return result.IsSuccessful;
        }
    }
}