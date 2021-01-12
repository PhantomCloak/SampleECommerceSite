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
    public class StoreController : ControllerBase
    {
        private readonly IUriGeneratorService _uriGeneratorService;
        private readonly IStoreService _storeService;
        private readonly IMapper _mapper;

        public StoreController(
            IUriGeneratorService uriGeneratorService,
            IMapper mapper,
            IStoreService storeService)
        {
            _uriGeneratorService = uriGeneratorService;
            _mapper = mapper;
            _storeService = storeService;
        }
        
        [AllowAnonymous]
        [HttpGet(ApiRoutes.Store.GetAll)]
        public async Task<IActionResult> GetAllStores([FromQuery]StoreFilterQuery filterQuery,[FromQuery] PaginationQuery paginationQuery)
        {
            var filter = _mapper.Map<StoreFilterParams>(filterQuery);
            var pagination = _mapper.Map<PaginationParams>(paginationQuery);

            var storeResult = await _storeService.GetAllStoresAsync(filter, pagination);
            
            var paginationResponse =
                PaginationHelpers.CreatePaginatedResponse(_uriGeneratorService, pagination,storeResult);

            return Ok(paginationResponse);
        }

        [AllowAnonymous]
        [HttpGet(ApiRoutes.Store.Get)]
        public async Task<IActionResult> GetStore(string storeName)
        {
            var store = await _storeService.GetStoreAsync(storeName);

            if (store == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<StoreDto>(store));
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPost(ApiRoutes.Store.Create)]
        public async Task<IActionResult> CreateStore(CreateStoreQuery createQuery)
        {
            var createStoreParams = _mapper.Map<CreateStoreParams>(createQuery);

            var createdStore = await _storeService.CreateStoreAsync(createStoreParams);
            
            var locationUri = _uriGeneratorService.GetStoreUri(createdStore.StoreName);
            return Created(locationUri, new Response<ProductDto>(_mapper.Map<ProductDto>(locationUri)));
        }
        
        [Authorize(Roles = "Admin")]
        [HttpDelete(ApiRoutes.Store.Delete)]
        public async Task<IActionResult> DeleteStore(string storeName)
        {
            bool status = await _storeService.DeleteStoreAsync(storeName);

            if (!status)
            {
                return Conflict("Store cannot be deleted.");
            }

            return Ok("Store deleted successfully.");
        }
        
    }
}