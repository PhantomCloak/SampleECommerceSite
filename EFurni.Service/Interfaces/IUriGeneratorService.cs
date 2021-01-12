using System;
using EFurni.Contract.V1.Queries;

namespace EFurni.Services
{
    public interface IUriGeneratorService
    {
        Uri GetProductUri(int postId);
        Uri GetBrandUri(string brandName);
        Uri GetCategoryUri(string categoryName);
        Uri GetOrderUri(int orderId);
        Uri GetStoreUri(string storeName);
        Uri GetCustomerUri(int customerId);
        
        Uri GetAllEntitiesUri(PaginationQuery pagination = null);
    }
}