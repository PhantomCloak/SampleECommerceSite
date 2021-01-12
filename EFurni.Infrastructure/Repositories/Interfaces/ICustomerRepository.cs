using System.Collections.Generic;
using System.Threading.Tasks;
using EFurni.Contract.V1.Queries;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Shared.Models;

namespace EFurni.Infrastructure.Repositories
{
    public interface ICustomerRepository <in TFilter> :
        IQueryFilter<TFilter, Customer>,
        IQuerySorter<TFilter, Customer> where TFilter : class
    {
        Task<IEnumerable<Customer>> GetAllCustomersAsync(TFilter filterParams,PaginationParams paginationParams = null);
        Task<bool> CreateCustomerAsync(Customer customer);
        Task<Customer> GetCustomerByIdAsync(int customerId);
        Task<Customer> GetCustomerByAccountId(int accountId);
        Task<bool> UpdateCustomerAsync(Customer customerToUpdate);
        Task<bool> DeleteCustomerAsync(int customerId);
    }
}