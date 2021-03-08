using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EFurni.Contract.V1.Queries.Create;
using EFurni.Contract.V1.Queries.Filter;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Infrastructure.Repositories;
using EFurni.Services;
using EFurni.Shared.DTOs;
using EFurni.Shared.Models;

namespace EFurni.Services
{
    internal class ProductReviewService : IProductReviewService
    {
        private readonly IProductRepository<ProductFilterParams> _productRepository;
        private readonly ICustomerRepository<CustomerFilterParams> _customerRepository;
        private readonly IMapper _mapper;

        public ProductReviewService(
            IProductRepository<ProductFilterParams> productRepository,
            ICustomerRepository<CustomerFilterParams> customerRepository,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CustomerReviewDto>> GetReviewsAsync(int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);

            return _mapper.Map<IEnumerable<CustomerReviewDto>>(product.CustomerReview);
        }

        public async Task<CustomerReviewDto> CreateReviewAsync(CreateProductReviewParams createReviewParams)
        {
            var product = await _productRepository.GetProductByIdAsync(createReviewParams.ProductId);
            var customer = await _customerRepository.GetCustomerByAccountId(createReviewParams.AuthorAccountId);

            if (product == null)
            {
                throw new Exception("Specified product was not found");
            }

            var productReview = new CustomerReview
            {
                CustomerId = customer.CustomerId,
                CustomerComment = createReviewParams.ReviewText,
                CustomerRating = createReviewParams.Rating
            };

            product.CustomerReview.Add(productReview);
    
            await _productRepository.UpdateProductAsync(product);

            return _mapper.Map<CustomerReviewDto>(productReview);
        }

        public Task<bool> UpdateReview(CustomerReviewDto reviewDto)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> DeleteReview(int productId)
        {
            throw new System.NotImplementedException();
        }
    }
}