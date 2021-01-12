//Disable possibly null check

#pragma warning disable 8601
#pragma warning disable 8603

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EFurni.Contract.V1.Queries.Filter;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Infrastructure.Repositories;
using EFurni.Shared.DTOs;
using EFurni.Shared.Models;
using EFurni.Shared.Types;

namespace EFurni.Services
{
    internal class OrderService : IOrderService
    {
        private readonly IPostalServiceRepository<PostalCompanyFilterParams> _postalServiceRepository;
        private readonly IOrderRepository<OrderFilterParams> _orderRepository;
        private readonly ICustomerRepository<CustomerFilterParams> _customerRepository;
        private readonly IProductRepository<ProductFilterParams> _productRepository;
        private readonly ILocationService _locationService;
        private readonly IMapper _mapper;

        public OrderService(
            IPostalServiceRepository<PostalCompanyFilterParams> postalServiceRepository,
            IOrderRepository<OrderFilterParams> orderRepository,
            ICustomerRepository<CustomerFilterParams> customerRepository,
            IProductRepository<ProductFilterParams> productRepository,
            ILocationService locationService,
            IMapper mapper)
        {
            _postalServiceRepository = postalServiceRepository;
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _locationService = locationService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CustomerOrderDto>> GetAllOrdersAsync(OrderFilterParams filterParams, PaginationParams paginationQuery)
        {
            var ordersResult = await _orderRepository.GetAllOrdersAsync(filterParams, paginationQuery);

            return _mapper.Map<IEnumerable<CustomerOrderDto>>(ordersResult);
        }

        public async Task<CustomerOrderDto> GetOrderAsync(int orderId)
        {
            var orderResult = await _orderRepository.GetOrderAsync(orderId);

            return _mapper.Map<CustomerOrderDto>(orderResult);
        }

        public async Task<CustomerOrderDto> CreateOrderAsync(CreateOrderParams orderParams)
        {
            var senderCustomer = await _customerRepository.GetCustomerByAccountId(orderParams.AccountId);
            var selectedPostalService = await _postalServiceRepository.GetPostalServiceByNameAsync(orderParams.PostalServiceName);
            var requestedProducts = await GetRequestedProducts(orderParams.Products);
            var deliveryAddress = await _locationService.GetAddressFromZip(orderParams.DeliveryZipCode);

            if (deliveryAddress == null)
            {
                throw new Exception("Zip code does not exist"); //todo make better error handling
            }

            if (requestedProducts.Count() != orderParams.Products.Length)
            {
                //Presentation should be handle this kind of situation not api itself
                throw new Exception("One or more products are does not exist or out of stock");
            }

            var customerOrderItems = new List<CustomerOrderItem>();
            foreach (var orderItem in orderParams.Products)
            {
                Product requestedProduct = requestedProducts.First(x => x.ProductId == orderItem.ProductId);
                customerOrderItems.Add(new CustomerOrderItem
                {
                    Product = requestedProduct,
                    ListPrice = requestedProduct.ListPrice,
                    ProductDiscount = requestedProduct.Discount,
                    Quantity = orderParams.Products.First(x => x.ProductId == orderItem.ProductId).Quantity,
                    StoreId = orderItem.StoreId
                });
            }

            var newOrderAddress = new CustomerOrderAddress()
            {
                CountryTag = deliveryAddress.CountryTag,
                Province = deliveryAddress.Province,
                District = deliveryAddress.District,
                Neighborhood = deliveryAddress.District,
                AddressTextPrimary = orderParams.DeliveryAddress,
                DestinationZipCode = orderParams.DeliveryZipCode
            };

            var newOrder = new CustomerOrder
            {
                ReceiverFirst = orderParams.FirstName,
                ReceiverLast = orderParams.SecondName,
                PhoneNumber = orderParams.PhoneNumber,
                OptionalMail = orderParams.OptionalMail,
                PostalServiceId = selectedPostalService.ServiceId,
                CustomerId = senderCustomer.CustomerId,
                OrderStatus = OrderStatus.Preparing,
                OrderDate = DateTime.Now,
                RequiredDate = DateTime.Now.AddDays((double) selectedPostalService.AvgDeliveryDay), //TODO implement new system
                CargoPrice = selectedPostalService.Price,
                AdditionalNote = orderParams.AdditionalNote,
                CustomerOrderItem = customerOrderItems,
                CustomerOrderAddress = newOrderAddress
            };

            bool createResult = await _orderRepository.CreateOrderAsync(newOrder);

            if (!createResult)
            {
                throw new Exception("Order couldn't created.");
            }

            var createdOrder = await _orderRepository.GetOrderAsync(newOrder.OrderId);

            return _mapper.Map<CustomerOrderDto>(createdOrder);
        }

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            return await _orderRepository.DeleteOrderAsync(orderId);
        }

        private async Task<IEnumerable<Product>> GetRequestedProducts(CreateOrderProductParams[] requestedProducts)
        {
            var productIds = requestedProducts.Select(x => x.ProductId).ToArray();

            var products = (await _productRepository.GetAllProductsAsync(new ProductFilterParams
            {
                ProductIds = productIds
            })).ToArray();

            if (products.Length != productIds.Length)
            {
                return Enumerable.Empty<Product>();
            }

            var stocks = products.SelectMany(x => x.Stock).ToArray();

            var productList = new List<Product>();
            foreach (var requestProduct in requestedProducts)
            {
                var userRequestedStock = stocks.FirstOrDefault(x => x.ProductId == requestProduct.ProductId && x.StoreId == requestProduct.StoreId);

                if (userRequestedStock == null)
                {
                    return Enumerable.Empty<Product>();
                }

                if (userRequestedStock.Quantity - requestProduct.Quantity <= 0)
                {
                    return Enumerable.Empty<Product>();
                }

                productList.Add(products.First(x => x.ProductId == userRequestedStock.ProductId));
            }

            return productList;
        }
    }
}