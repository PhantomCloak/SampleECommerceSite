using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EFurni.Contract.V1.Queries.Create;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Infrastructure.Repositories;
using EFurni.Shared.DTOs;
using EFurni.Shared.Models;

namespace EFurni.Services
{
    internal class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository<CustomerFilterParams> _customerRepository;
        private readonly IMapper _mapper;
        
        public CustomerService(ICustomerRepository<CustomerFilterParams> customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync(CustomerFilterParams customerFilterParams = null, PaginationParams paginationParams = null)
        {
            var customers = await _customerRepository.GetAllCustomersAsync(customerFilterParams, paginationParams);

            return _mapper.Map<IEnumerable<CustomerDto>>(customers);
        }

        public async Task<CustomerDto> GetCustomerAsync(int customerId)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(customerId);

            return _mapper.Map<CustomerDto>(customer);
        }

        public async Task<CustomerDto> GetCustomerByAccountIdAsync(int accountId)
        {
            var customer = await _customerRepository.GetCustomerByAccountId(accountId);

            return _mapper.Map<CustomerDto>(customer);
        }

        public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerParams createCustomerParams)
        {
            var customer = new Customer()
            {
                FirstName = createCustomerParams.FirstName,
                LastName = createCustomerParams.LastName,
                ProfilePictureUrl = createCustomerParams.ProfilePictureUrl,
                PhoneNumber = createCustomerParams.PhoneNumber
            };

            await _customerRepository.CreateCustomerAsync(customer);
            
            return _mapper.Map<CustomerDto>(customer);
        }

        public async Task<bool> DeleteCustomerAsync(int customerId)
        {
            
            return await _customerRepository.DeleteCustomerAsync(customerId);
        }
    }
}