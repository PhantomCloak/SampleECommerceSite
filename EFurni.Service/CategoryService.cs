using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EFurni.Contract.V1.Queries.Create;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Infrastructure.Repositories;
using EFurni.Shared.DTOs;
using EFurni.Shared.Models;

namespace EFurni.Services
{
    internal class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository<CategoryFilterParams> _categoryRepository;
        private readonly IMapper _mapper;
        
        public CategoryService(ICategoryRepository<CategoryFilterParams> categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(CategoryFilterParams categoryFilterParams = null, PaginationParams paginationParams = null)
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync(categoryFilterParams, paginationParams);

            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto> GetCategoryAsync(string categoryName)
        {
            var category = await _categoryRepository.GetCategoryByNameAsync(categoryName);

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryParams createCategoryParams)
        {
            var newCategory = new Category()
            {
                CategoryName = createCategoryParams.CategoryName
            };
            
            await _categoryRepository.CreateCategoryAsync(newCategory);

            return _mapper.Map<CategoryDto>(newCategory);
        }

        public async Task<bool> DeleteCategoryAsync(string categoryName)
        {
            return await _categoryRepository.DeleteCategoryAsync(categoryName);
        }
    }
}