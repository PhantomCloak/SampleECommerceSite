using System.Collections.Generic;
using System.Threading.Tasks;
using EFurni.Contract.V1.Queries;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Infrastructure.OutputDevices;
using EFurni.Shared.Models;

namespace EFurni.Infrastructure.Repositories
{
    public interface IProductRepository<in TFilter> :
        IQueryFilter<TFilter, Product>,
        IQuerySorter<TFilter, Product> where TFilter : class
    {
        Task<IEnumerable<Product>> GetAllProductsAsync(TFilter filter = null, PaginationParams paginationParams = null);
        Task<bool> CreateProductAsync(Product product);
        Task<Product> GetProductByIdAsync(int productId);
        Task<bool> UpdateProductAsync(Product productToUpdate);
        Task<bool> DeleteProductAsync(int productId);
        Task DisposeEntities(IEnumerable<Product> entities);
        void AttachOutputDevice(IRepositoryQueryOutputDevice repositoryQueryOutputDevice);
    }
}