using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using EFurni.Contract.V1;
using EFurni.Contract.V1.Queries;
using EFurni.Contract.V1.Queries.Filter;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Contract.V1.Responses;
using EFurni.Core.Authentication;
using EFurni.Core.Helpers;
using EFurni.Services;
using EFurni.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EFurni.Core.Controllers.V1
{
    [ApiController]
    [Authorize(Roles = "Trusted,Admin")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IUriGeneratorService _uriGeneratorService;
        private readonly IAuthenticationContext _authenticationContext;
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public OrderController(
            IOrderService orderService,
            IUriGeneratorService uriGeneratorService,
            IAuthenticationContext authenticationContext,
            ICustomerService customerService,
            IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
            _customerService = customerService;
            _authenticationContext = authenticationContext;
            _uriGeneratorService = uriGeneratorService;

            _authenticationContext.AttachToController(this);
        }

        [HttpGet(ApiRoutes.Order.GetAll)]
        public async Task<IActionResult> GetAllOrders(
            [FromQuery] OrderFilterQuery filterQuery,
            [FromQuery] PaginationQuery paginationQuery)
        {
            int customerId = User.GetClaim<int>(ClaimTypes.NameIdentifier);

            var filter = _mapper.Map<OrderFilterParams>(filterQuery);
            var pagination = _mapper.Map<PaginationParams>(paginationQuery);

            filter.CustomerId = customerId;
            
            var resultOrders = (await _orderService.GetAllOrdersAsync(filter,pagination)).ToArray();
            
            var paginationResponse =
                PaginationHelpers.CreatePaginatedQueryResponse(_uriGeneratorService, pagination, resultOrders,resultOrders.Count());
            
            return Ok(paginationResponse);
        }

        [HttpGet(ApiRoutes.Order.Get)]
        public async Task<IActionResult> GetOrder(int orderId)
        {
            int accountId = _authenticationContext.GetCurrentActorIdentifier();

            var customer = await _customerService.GetCustomerByAccountIdAsync(accountId);

            if (customer == null)
            {
                return NotFound();
            }
            
            var order = await _orderService.GetOrderAsync(orderId);

            if (order == null)
            {
                return NotFound();
            }
            
            if (order.CustomerId != customer.CustomerId)
            {
                return NotFound();
            }
            
            return Ok(new Response<CustomerOrderDto>(order));
        }

        [HttpPost(ApiRoutes.Order.Create)]
        public async Task<IActionResult> CreateOrder([FromBody]CreateOrderQuery createQuery)
        {
            int customerId = User.GetClaim<int>(ClaimTypes.NameIdentifier);
            var queryParams = _mapper.Map<CreateOrderParams>(createQuery);
            queryParams.AccountId = customerId;

            CustomerOrderDto createdOrder;

            try
            {
                createdOrder = await _orderService.CreateOrderAsync(queryParams);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            var locationUri = _uriGeneratorService.GetOrderUri(createdOrder.OrderId);

            return Created(locationUri, new Response<CustomerOrderDto>(createdOrder));
        }

        [HttpDelete(ApiRoutes.Order.Delete)]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            int accountId = _authenticationContext.GetCurrentActorIdentifier();

            var customer = await _customerService.GetCustomerByAccountIdAsync(accountId);

            if (customer == null)
            {
                return NotFound();
            }
            
            var order = await _orderService.GetOrderAsync(orderId);
            
            if (order.CustomerId != accountId)
            {
                return Unauthorized();
            }
            
            bool result = await _orderService.DeleteOrderAsync(orderId);

            if (!result)
            {
                BadRequest();
            }

            return Ok();
        }
    }
}