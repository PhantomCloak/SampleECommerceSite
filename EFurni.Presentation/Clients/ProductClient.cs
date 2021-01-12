#pragma warning disable 8625
#pragma warning disable 8625
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EFurni.Contract.V1;
using EFurni.Contract.V1.Queries;
using EFurni.Contract.V1.Responses;
using EFurni.Presentation.Extensions;
using EFurni.Presentation.RestClientExtensions;
using EFurni.Shared.DTOs;
using RestSharp;
namespace EFurni.Presentation.Clients.ClientInterfaces
{
    public class ProductClient : IProductClient
    {
        private readonly RestClient _client;

        public ProductClient(RestClientManager configuration)
        {
            _client = configuration.GetRestClient();
        }

        public async Task<(IEnumerable<ProductDto>,int)> GetProductsAsync(
            ProductFilterQuery query = null,
            int? pageSize = null,
            int? page = null)
        {
            
            var productRequest = new RestRequest(ApiRoutes.Product.GetAll);

            if (query != null)
            {
                productRequest.AddFilter(query);
            }

            if (page != null && pageSize != null)
            {
                productRequest.AddPagination(new PaginationQuery{PageSize = (int)pageSize, PageNumber = (int)page});
            }
            
            var requestedProducts = _client.Execute<PagedQueryResponse<ProductDto>>(productRequest);

            // StringBuilder sb = new StringBuilder();
            // sb.Append("\nError Msg: " + requestedProducts.ErrorMessage);
            // sb.Append("\nContent: " + requestedProducts.Content);
            // sb.Append("\nStatus Code : " + requestedProducts.StatusCode);
            // sb.Append("\nError Exception : " + requestedProducts.ErrorException.Message);
            //
            // File.WriteAllText("log.txt",sb.ToString());
            //
            return (requestedProducts.Data.Data,requestedProducts.Data.FetchedItems);
        }

        public async Task<ProductDto> GetProductAsync(int productId)
        {
            var basketDeleteRequest = new RestRequest(ApiRoutes.Product.Get, Method.GET)
                .AddParameter("productId", productId, ParameterType.UrlSegment);

            var response = await _client.ExecuteAsync<Response<ProductDto>>(basketDeleteRequest);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            return response.Data.Data;
        }
    }
}