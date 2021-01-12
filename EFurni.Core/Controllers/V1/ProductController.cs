using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EFurni.Contract.V1;
using EFurni.Contract.V1.Queries;
using EFurni.Contract.V1.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EFurni.Contract.V1.Queries.Create;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Core.Helpers;
using EFurni.Service.OutputDevices;
using EFurni.Services;
using EFurni.Shared.DTOs;

namespace EFurni.Core.Controllers.V1
{
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IUriGeneratorService _uriGeneratorService;
        private readonly IProductServiceOutputDevice _outputDevice;
        private readonly IMapper _mapper;

        public ProductController(
            IProductService productService,
            IUriGeneratorService uriGeneratorService,
            IProductServiceOutputDevice outputDevice,
            IMapper mapper)
        {
            _productService = productService;
            _uriGeneratorService = uriGeneratorService;
            _outputDevice = outputDevice;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet(ApiRoutes.Product.GetAll)]
        public async Task<IActionResult> GetAll(
            [FromQuery] ProductFilterQuery filterQuery,
            [FromQuery] PaginationQuery paginationQuery)
        {
            var pagination = _mapper.Map<PaginationParams>(paginationQuery);
            var filter = _mapper.Map<ProductFilterParams>(filterQuery);

            _productService.AttachOutputDevice(_outputDevice);
            
            var products = (await _productService.GetAllProductsAsync(filter,pagination)).ToArray();

            var queryResult = _outputDevice.GetFilteredProductCount();
            
            var paginationResponse =
                PaginationHelpers.CreatePaginatedQueryResponse(_uriGeneratorService, pagination, products,products.Length);

            paginationResponse.FetchedItems = queryResult.FilteredCount;
            paginationResponse.QueriedItems = queryResult.TotalCount;
                
            return Ok(paginationResponse);
        }

        [AllowAnonymous]
        [HttpGet(ApiRoutes.Product.Get)]
        public async Task<IActionResult> Get(int productId)
        {
            var product = await _productService.GetProductAsync(productId);
            
            return Ok(new Response<ProductDto>(product));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost(ApiRoutes.Product.Create)]
        public async Task<IActionResult> Create([FromBody] CreateProductQuery createQuery)
        {
            var createProductParams = _mapper.Map<CreateProductParams>(createQuery);

            var createdProduct = await _productService.CreateProductAsync(createProductParams);
            
            if (createdProduct == null)
            {
                return NotFound();
            }

            var locationUri = _uriGeneratorService.GetProductUri(createdProduct.ProductId);
            
            return Created(locationUri, new Response<ProductDto>(createdProduct));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete(ApiRoutes.Product.Delete)]
        public async Task<IActionResult> Delete(int productId)
        {
            var productToDelete = await _productService.DeleteProductAsync(productId);
            
            if (productToDelete == null)
            {
                return BadRequest("The specified query cannot perform the requested operation.");
            }

            return Ok("Product deleted successfully.");
        }
    }
}