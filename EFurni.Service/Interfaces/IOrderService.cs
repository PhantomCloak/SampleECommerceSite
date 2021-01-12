using System.Collections.Generic;
using System.Threading.Tasks;
using EFurni.Contract.V1.Queries;
using EFurni.Contract.V1.Queries.Filter;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Shared.DTOs;

namespace EFurni.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<CustomerOrderDto>> GetAllOrdersAsync(OrderFilterParams filterParams = null,PaginationParams paginationQuery = null);
        Task<CustomerOrderDto> GetOrderAsync(int orderId);
        Task<CustomerOrderDto> CreateOrderAsync(CreateOrderParams orderParams);
        Task<bool> DeleteOrderAsync(int orderId);
    }
}