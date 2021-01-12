using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using EFurni.Contract.V1;
using EFurni.Contract.V1.Queries;
using EFurni.Contract.V1.Queries.Create;
using EFurni.Contract.V1.Queries.Filter;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Contract.V1.Responses;
using EFurni.Core.Helpers;
using EFurni.Services;
using EFurni.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EFurni.Core.Controllers.V1
{
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IUriGeneratorService _uriGeneratorService;
        private readonly IMapper _mapper;
        
        public CustomerController(
            ICustomerService customerService,
            IMapper mapper, IUriGeneratorService uriGeneratorService)
        {
            _customerService = customerService;
            _uriGeneratorService = uriGeneratorService;
            _mapper = mapper;

        }
        
        [Authorize(Roles = "Trusted,Admin")]
        [HttpGet(ApiRoutes.Customer.GetAll)]
        public async Task<IActionResult> GetCustomers(
            [FromQuery]CustomerFilterQuery filterQuery,
            [FromQuery]PaginationQuery paginationQuery)
        {
            var pagination = _mapper.Map<PaginationParams>(paginationQuery);
            var filter = _mapper.Map<CustomerFilterQuery, CustomerFilterParams>(filterQuery);

            var customers = (await _customerService.GetAllCustomersAsync(filter, pagination)).ToArray();
            
            var paginationResponse =
                PaginationHelpers.CreatePaginatedQueryResponse(_uriGeneratorService, pagination, customers,customers.Count());

            return Ok(paginationResponse);
        }
        
        [Authorize(Roles = "Trusted,Admin")]
        [HttpGet(ApiRoutes.Customer.Get)]
        public async Task<IActionResult> GetCustomer(int customerId)
        {
            int accountId = User.GetClaim<int>(ClaimTypes.NameIdentifier);

            var senderCustomer = await _customerService.GetCustomerByAccountIdAsync(accountId);
            var requestedCustomer = await _customerService.GetCustomerAsync(customerId);

            if (requestedCustomer?.CustomerId != senderCustomer.CustomerId && !User.IsInRole("Admin"))
            {
                return Unauthorized();
            }
            
            return Ok(new Response<CustomerDto>(requestedCustomer));
        }
        
        [HttpPost(ApiRoutes.Customer.Create)]
        public async Task<IActionResult> CreateCustomer(CreateCustomerQuery createQuery)
        {
            var createCustomerParams = _mapper.Map<CreateCustomerParams>(createQuery);

            var createdCustomer = await _customerService.CreateCustomerAsync(createCustomerParams);
            
            var locationUri = _uriGeneratorService.GetCustomerUri(createdCustomer.CustomerId);

            return Created(locationUri, new Response<CustomerDto>(createdCustomer));
        }
        
        [Authorize(Roles = "Trusted,Admin")]
        [HttpDelete(ApiRoutes.Customer.Delete)]
        public async Task<IActionResult> DeleteCustomer(int customerId)
        {
            int accountId = User.GetClaim<int>(ClaimTypes.NameIdentifier);

            var senderCustomer = await _customerService.GetCustomerByAccountIdAsync(accountId);
            var requestedCustomer = await _customerService.GetCustomerAsync(customerId);

            if (requestedCustomer?.CustomerId != senderCustomer.CustomerId && !User.IsInRole("Admin"))
            {
                return Unauthorized();
            }

            bool result = await _customerService.DeleteCustomerAsync(customerId);

            if (!result)
            {
                return Conflict();
            }

            return Ok(new Response<string>("Customer deleted successfully."));
        }
        
    }
}