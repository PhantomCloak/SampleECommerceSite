#pragma warning disable 8625
using System;
using EFurni.Contract.V1;
using EFurni.Contract.V1.Queries;
using Microsoft.AspNetCore.WebUtilities;

namespace EFurni.Services
{
    internal class UriGeneratorService : IUriGeneratorService
    {
        private readonly string _baseUri;
        
        public UriGeneratorService(string baseUri)
        {
            _baseUri = baseUri;
        }
        
        public Uri GetProductUri(int postId)
        {
            return new Uri(_baseUri + ApiRoutes.Product.Get.Replace("{productId}", postId.ToString()));
        }

        public Uri GetBrandUri(string brandName)
        {
            return new Uri(_baseUri + ApiRoutes.Brand.Get.Replace("{brandName}",brandName));
        }

        public Uri GetCategoryUri(string categoryName)
        {
            return new Uri(_baseUri + ApiRoutes.Brand.Get.Replace("{categoryName}",categoryName));
        }

        public Uri GetOrderUri(int orderId)
        {
            return new Uri(_baseUri + ApiRoutes.Order.Get.Replace("{orderId}",orderId.ToString()));
        }

        public Uri GetStoreUri(string storeName)
        {
            return new Uri(_baseUri + ApiRoutes.Store.Get.Replace("{storeName}",storeName));
        }

        public Uri GetCustomerUri(int customerId)
        {
            return new Uri(_baseUri + ApiRoutes.Customer.Get.Replace("{storeName}",customerId.ToString()));
        }

        public Uri GetAllEntitiesUri(PaginationQuery pagination = null)
        {
            var modifiedUri = QueryHelpers.AddQueryString(_baseUri, "pageNumber", pagination.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", pagination.PageSize.ToString());
            
            return new Uri(modifiedUri);
        }
    }
}