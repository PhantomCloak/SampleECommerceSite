using System.Threading.Tasks;
using AutoMapper;
using EFurni.Contract.V1;
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
    public class BrandController : Controller
    {
        private readonly IBrandService _brandService;
        private readonly IUriGeneratorService _uriGeneratorService;
        private readonly IMapper _mapper;

        public BrandController(
            IBrandService brandService,
            IUriGeneratorService uriGeneratorService,
            IMapper mapper)
        {
            _brandService = brandService;
            _uriGeneratorService = uriGeneratorService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet(ApiRoutes.Brand.GetAll)]
        public async Task<IActionResult> GetAllBrands([FromQuery] BrandFilterQuery filterQuery,[FromQuery] PaginationQuery paginationQuery)
        {
            var pagination = _mapper.Map<PaginationParams>(paginationQuery);
            var filter = _mapper.Map<BrandFilterParams>(filterQuery);

            var brands = await _brandService.GetAllBrandsAsync(filter, pagination);

            var paginationResponse =
                PaginationHelpers.CreatePaginatedResponse(_uriGeneratorService, pagination,brands);

            return Ok(paginationResponse);
        }

        [AllowAnonymous]
        [HttpGet(ApiRoutes.Brand.Get)]
        public async Task<IActionResult> GetBrand(string brandName)
        {
            var brand = await _brandService.GetBrandAsync(brandName);

            if (brand == null)
            {
                return NotFound(new Response<string>("The specified brand was not found."));
            }

            return Ok(new Response<BrandDto>(brand));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost(ApiRoutes.Brand.Create)]
        public async Task<IActionResult> CreateBrand([FromBody] CreateBrandQuery createQuery)
        {
            var createOrderParams = _mapper.Map<CreateBrandParams>(createQuery);
            
            var createdBrand = await _brandService.CreateBrandAsync(createOrderParams);

            if (createdBrand == null)
            {
                return BadRequest("The specified query cannot perform the requested operation.");
            }

            var locationUri = _uriGeneratorService.GetBrandUri(createdBrand.BrandName);
            return Created(locationUri, new Response<BrandDto>(createdBrand));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete(ApiRoutes.Brand.Delete)]
        public async Task<IActionResult> DeleteBrand(string brandName)
        {
            var result = await _brandService.DeleteBrandAsync(brandName);

            if (!result)
            {
                return NotFound(new Response<string>("The specified brand was not found."));
            }

            return Ok(new Response<string>("Brand deleted successfully."));
        }

    }
}