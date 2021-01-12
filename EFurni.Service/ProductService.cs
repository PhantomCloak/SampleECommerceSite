using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EFurni.Contract.V1.Queries.Create;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Infrastructure;
using EFurni.Infrastructure.OutputDevices;
using EFurni.Infrastructure.Repositories;
using EFurni.Service.OutputDevices;
using EFurni.Services;
using EFurni.Shared.DTOs;
using EFurni.Shared.Models;

namespace EFurni.Core.Services
{
    internal class ProductService : IProductService
    {
        private readonly IProductRepository<ProductFilterParams> _productRepository;
        private readonly IBrandRepository<BrandFilterParams> _brandRepository;
        private readonly ICategoryRepository<CategoryFilterParams> _categoryRepository;
        private readonly IRepositoryQueryOutputDevice _repositoryOutputDeviceDevice;
        private readonly IMapper _mapper;
        private IProductServiceOutputDevice _outputDeviceDevice;
        
        public ProductService(
            IProductRepository<ProductFilterParams> productRepository,
            IBrandRepository<BrandFilterParams> brandRepository,
            ICategoryRepository<CategoryFilterParams> categoryRepository,
            IMapper mapper, IRepositoryQueryOutputDevice repositoryOutputDeviceDevice)
        {
            _productRepository = productRepository;
            _brandRepository = brandRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _repositoryOutputDeviceDevice = repositoryOutputDeviceDevice;
        }
        
        private bool OutputDeviceEnabled => _repositoryOutputDeviceDevice != null && _outputDeviceDevice != null;
        
        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync(ProductFilterParams productFilterParams = null,PaginationParams paginationParams =  null)
        {
            if (OutputDeviceEnabled)
                _productRepository.AttachOutputDevice(_repositoryOutputDeviceDevice);
            
            var products = await _productRepository.GetAllProductsAsync(productFilterParams,paginationParams);
            
            WriteOutputDevice();
            
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto> GetProductAsync(int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductParams productParams)
        {
            var resolvedBrand = await _brandRepository.GetBrandByNameAsync(productParams.BrandName);
            var resolvedCategory = await _categoryRepository.GetCategoryByNameAsync(productParams.CategoryName);

            if (resolvedBrand == null || resolvedCategory == null)
            {
                throw new Exception("The specified brand or category doesn't exist.");
            }

            var newProduct = new Product
            {
                ProductName = productParams.ProductName,
                SubType = productParams.SubType,
                Category = resolvedCategory,
                ProductImage = productParams.ProductImage,
                ProductWeight = productParams.ProductWeight,
                ProductHeight = productParams.ProductHeight,
                ProductWidth = productParams.ProductWidth,
                BoxPieces = productParams.BoxPieces,
                ModelYear = productParams.ModelYear,
                ListPrice = productParams.ListPrice,
                Description = productParams.Description,
                Brand = resolvedBrand
            };
            
            bool createResult = await _productRepository.CreateProductAsync(newProduct);

            if (!createResult)
            {
                throw new Exception("Failed to add Product to database.");
            }

            return _mapper.Map<ProductDto>(newProduct);
        }

        public Task<bool> UpdateProductAsync(ProductDto productDto)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            var productToDelete = await _productRepository.GetProductByIdAsync(productId);

            if (productToDelete == null)
            {
                return false;
            }

            return true;
        }

        public void AttachOutputDevice(IProductServiceOutputDevice outputDeviceDevice)
        {
            _outputDeviceDevice = outputDeviceDevice;
        }

        private void WriteOutputDevice()
        {
            if (OutputDeviceEnabled)
            {
                _outputDeviceDevice.SetQueryResultCount(_repositoryOutputDeviceDevice.GetQueryResultCount(),_repositoryOutputDeviceDevice.GetQueryTotalCount());
            }
        }
    }
}