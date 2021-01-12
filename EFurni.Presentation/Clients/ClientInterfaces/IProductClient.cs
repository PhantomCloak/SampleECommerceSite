using System.Collections.Generic;
using System.Threading.Tasks;
using EFurni.Contract.V1.Queries;
using EFurni.Shared.DTOs;

namespace EFurni.Presentation.Clients.ClientInterfaces
{
    public interface IProductClient
    {
        Task<(IEnumerable<ProductDto> Products,int FilterCount)> GetProductsAsync(ProductFilterQuery query=null,int? pageSize = null,int? page = null);
        Task<ProductDto> GetProductAsync(int productId);
    }
}