using System.Collections.Generic;
using System.Threading.Tasks;
using EFurni.Contract.V1.Queries.Create;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Shared.DTOs;

namespace EFurni.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDto>> GetAllCustomersAsync(CustomerFilterParams customerFilterParams = null,PaginationParams paginationParams = null);
        Task<CustomerDto> GetCustomerAsync(int customerId);
        Task<CustomerDto> GetCustomerByAccountIdAsync(int accountId);
        Task<CustomerDto> CreateCustomerAsync(CreateCustomerParams createCustomerParams);
        Task<bool> DeleteCustomerAsync(int customerId);
    }
}