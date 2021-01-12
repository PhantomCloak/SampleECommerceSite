using System.Threading.Tasks;
using AutoMapper;
using EFurni.Contract.V1;
using EFurni.Contract.V1.Extensions;
using EFurni.Contract.V1.Queries;
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
    public class PostalCodeController : ControllerBase
    {
        private readonly ILocationService _locationService;
        private readonly IUriGeneratorService _uriGeneratorService;
        private readonly IMapper _mapper;

        public PostalCodeController(ILocationService locationService,
            IUriGeneratorService uriGeneratorService,
            IMapper mapper)
        {
            _locationService = locationService;
            _uriGeneratorService = uriGeneratorService;
            _mapper = mapper;
        }
        
        [HttpGet(ApiRoutes.Location.GetCountries)]
        public async Task<IActionResult> GetCountries([FromQuery] PaginationQuery paginationQuery)
        {
            var pagination = _mapper.Map<PaginationParams>(paginationQuery);

            var result = await _locationService.GetCountriesAsync(pagination);

            if (pagination == null || !pagination.IsValidPagination())
            {
                return Ok(new PagedResponse<CountryDto>(result));
            }

            var paginatedResponse = PaginationHelpers.CreatePaginatedResponse(_uriGeneratorService, pagination, result);

            return Ok(paginatedResponse);
        }

        [HttpGet(ApiRoutes.Location.GetProvince)]
        public async Task<IActionResult> GetProvinces(
            string countryTag,
            [FromQuery] PaginationQuery paginationQuery)
        {
            var pagination = _mapper.Map<PaginationParams>(paginationQuery);

            var result = await _locationService.GetProvincesAsync(countryTag, pagination);

            if (pagination == null || !pagination.IsValidPagination())
            {
                return Ok(new PagedResponse<ProvinceDto>(result));
            }

            var paginatedResponse = PaginationHelpers.CreatePaginatedResponse(_uriGeneratorService, pagination, result);

            return Ok(paginatedResponse);
        }

        [HttpGet(ApiRoutes.Location.GetDistrict)]
        public async Task<IActionResult> GetDistricts(
            string countryTag, 
            string provinceName,
            [FromQuery] PaginationQuery paginationQuery)
        {
            var pagination = _mapper.Map<PaginationParams>(paginationQuery);

            var result = await _locationService.GetDistrictsAsync(countryTag, provinceName, pagination);

            if (pagination == null || !pagination.IsValidPagination())
            {
                return Ok(new PagedResponse<DistrictDto>(result));
            }

            var paginatedResponse = PaginationHelpers.CreatePaginatedResponse(_uriGeneratorService, pagination, result);

            return Ok(paginatedResponse);
        }

        [HttpGet(ApiRoutes.Location.GetNeighborhood)]
        public async Task<IActionResult> GetNeighborhoods(
            string countryTag,
            string provinceName,
            string districtName,
            [FromQuery] PaginationQuery paginationQuery)
        {
            var pagination = _mapper.Map<PaginationParams>(paginationQuery);

            var result = await _locationService.GetNeighborhoodsAsync(countryTag,provinceName,districtName, pagination);

            if (pagination == null || !pagination.IsValidPagination())
            {
                return Ok(new PagedResponse<NeighborhoodDto>(result));
            }

            var paginatedResponse = PaginationHelpers.CreatePaginatedResponse(_uriGeneratorService, pagination, result);

            return Ok(paginatedResponse);
        }
    }
}