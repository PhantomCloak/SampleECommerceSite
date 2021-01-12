using System.Threading.Tasks;
using EFurni.Contract.V1.Queries;
using EFurni.Shared.DTOs;

namespace EFurni.Presentation.Clients.ClientInterfaces
{
    public interface IOrderClient
    {
        Task<CustomerOrderDto> CreateOrder(CreateOrderQuery createOrderQuery);
        Task<CustomerOrderDto> GetOrder(int orderId);
    }
}