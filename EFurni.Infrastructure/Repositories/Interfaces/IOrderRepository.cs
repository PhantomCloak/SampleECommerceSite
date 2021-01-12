using System.Collections.Generic;
using System.Threading.Tasks;
using EFurni.Contract.V1.Queries;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Shared.Models;

namespace EFurni.Infrastructure.Repositories
{
    public interface IOrderRepository <in TFilter> :
        IQueryFilter<TFilter, CustomerOrder> where TFilter : class
    {
        Task<IEnumerable<CustomerOrder>> GetAllOrdersAsync(TFilter filter = null, PaginationParams paginationParams = null);
        Task<CustomerOrder> GetOrderAsync(int orderId);
        Task<bool> CreateOrderAsync(CustomerOrder order);
        Task<bool> UpdateOrderAsync(CustomerOrder order);
        Task<bool> DeleteOrderAsync(int orderId);
    }
}