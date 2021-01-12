using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Infrastructure.Repositories;
using EFurni.Shared.DTOs;
using EFurni.Shared.Models;
using Microsoft.Extensions.Configuration;

namespace EFurni.Services
{
    internal class SummaryService : ISummaryService
    {
        private readonly IBrandRepository<BrandFilterParams> _brandRepository;
        private readonly IProductRepository<ProductFilterParams> _productRepository;
        private readonly ICategoryRepository<CategoryFilterParams> _categoryRepository;
        
        public SummaryService(
            ICategoryRepository<CategoryFilterParams> categoryRepository,
            IBrandRepository<BrandFilterParams> brandRepository,
            IProductRepository<ProductFilterParams> productRepository)
        {
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Info>> GetAllInformationAsync()
        {
            return await GenerateInfoAsync();
        }

        public async Task<Info> GetInformationByNameAsync(string infoName)
        {
            var info = (await GenerateInfoAsync()).FirstOrDefault(x => x.InfoName == infoName);

            return info;
        }

        public async Task<Info[]> GenerateInfoAsync()
        {
            var products = await _productRepository.GetAllProductsAsync();
            var brands = await _brandRepository.GetAllBrandsAsync();
            var categories = await _categoryRepository.GetAllCategoriesAsync();

            //for enumerate once
            var productArray = products as Product[] ?? products.ToArray();
            var brandArray = brands as Brand[] ?? brands.ToArray();
            var categoryArray = categories as Category[] ?? categories.ToArray();

            return new Info[]
            {
                new ProductInfoDto()
                {
                    InfoName = "productInfo",
                    TotalProducts = productArray.Length,
                    MinPriceRange = productArray.Min(x => x.ListPrice),
                    MaxPriceRange = productArray.Max(x => x.ListPrice),
                    MinYearRange = productArray.Min(x => x.ModelYear),
                    MaxYearRange = productArray.Max(x => x.ModelYear),
                    AvailableColors = productArray.Select(x => x.ProductColor).Distinct()
                },
                new BrandInfoDto()
                {
                    InfoName = "brandInfo",
                    BrandNames = brandArray.Select(x => x.BrandName),
                    TotalProductCounts = brandArray.Select(x => x.Product.Count)
                },
                new CategoryInfoDto()
                {
                    InfoName = "categoryInfo",
                    CategoryNames = categoryArray.Select(x => x.CategoryName),
                    TotalProductCounts = categoryArray.Select(x => x.Product.Count)
                }
            };
        }
    }
}