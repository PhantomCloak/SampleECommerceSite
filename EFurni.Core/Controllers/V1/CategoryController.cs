using System.Threading.Tasks;
using AutoMapper;
using EFurni.Contract.V1;
using EFurni.Contract.V1.Extensions;
using EFurni.Contract.V1.Queries;
using EFurni.Contract.V1.Queries.Create;
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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IUriGeneratorService _uriGeneratorService;
        private readonly IMapper _mapper;

        public CategoryController(
            ICategoryService categoryService,
            IUriGeneratorService uriGeneratorService,
            IMapper mapper)
        {
            _categoryService = categoryService;
            _uriGeneratorService = uriGeneratorService;
            _mapper = mapper;
        }


        [HttpGet(ApiRoutes.Category.GetAll)]
        public async Task<IActionResult> GetAllCategories(
            [FromQuery] CategoryFilterQuery filterQuery,
            [FromQuery] PaginationQuery paginationQuery)
        {
            var pagination = _mapper.Map<PaginationParams>(paginationQuery);
            var filter = _mapper.Map<CategoryFilterParams>(filterQuery);

            var categories = await _categoryService.GetAllCategoriesAsync(filter, pagination);

            if (pagination == null || pagination.IsValidPagination())
            {
                return Ok(new PagedResponse<CategoryDto>(categories));
            }

            var paginationResponse =
                PaginationHelpers.CreatePaginatedResponse(_uriGeneratorService, pagination, categories);

            return Ok(paginationResponse);
        }

        [HttpGet(ApiRoutes.Category.Get)]
        public async Task<IActionResult> GetCategory(string categoryName)
        {
            var category = await _categoryService.GetCategoryAsync(categoryName);

            if (category == null)
            {
                return NotFound(new Response<string>("The specified category was not found."));
            }

            return Ok(new Response<CategoryDto>(category));
        }
        
        // [Authorize(Roles = "Admin")]
        [HttpPost(ApiRoutes.Category.Create)]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryQuery createQuery)
        {
            var createCategoryParams = _mapper.Map<CreateCategoryParams>(createQuery);
            
            var createdCategory = await _categoryService.CreateCategoryAsync(createCategoryParams);

            if (createdCategory == null)
            {
                return BadRequest(new Response<string>("The specified query cannot perform the requested operation."));
            }

            var locationUri = _uriGeneratorService.GetCategoryUri(createQuery.CategoryName);
            return Created(locationUri, new Response<CategoryDto>(createdCategory));
        }
        
        // [Authorize(Roles = "Admin")]
        [HttpDelete(ApiRoutes.Category.Delete)]
        public async Task<IActionResult> DeleteCategory(string categoryName)
        {
            bool result = await _categoryService.DeleteCategoryAsync(categoryName);

            if (result == null)
            {
                return NotFound(new Response<string>("The specified category was not found."));
            }

            return Ok(new Response<string>("Category deleted successfully."));
        }

    }
}