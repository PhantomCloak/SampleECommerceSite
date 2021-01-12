using System.Collections.Generic;
using System.Threading.Tasks;
using EFurni.Contract.V1.Queries.Create;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Service.OutputDevices;
using EFurni.Shared.DTOs;

namespace EFurni.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync(ProductFilterParams productFilterParams = null,PaginationParams paginationParams = null);
        Task<ProductDto> GetProductAsync(int productId);
        Task<ProductDto> CreateProductAsync(CreateProductParams productParams);
        Task<bool> UpdateProductAsync(ProductDto productDto);
        Task<bool> DeleteProductAsync(int productId);
        void AttachOutputDevice(IProductServiceOutputDevice outputDeviceDevice);
    }
}