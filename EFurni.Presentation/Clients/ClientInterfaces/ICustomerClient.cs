using System.Collections.Generic;
using System.Threading.Tasks;
using EFurni.Shared.DTOs;

namespace EFurni.Presentation.Clients.ClientInterfaces
{
    public interface ICustomerClient
    {
        public Task<CustomerDto> GetCustomer(int customerId);
        public Task<CustomerDto> GetSelfAsync();
    }
}