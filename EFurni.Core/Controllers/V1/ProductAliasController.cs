using System.Threading.Tasks;
using EFurni.Contract.V1;
using EFurni.Services;
using EFurni.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EFurni.Core.Controllers.V1
{
    [ApiController]
    public class ProductAliasController : ControllerBase
    {
        private readonly ISummaryService _summaryService;
     
        public ProductAliasController(ISummaryService summaryService)
        {
            _summaryService = summaryService;
        }
        
        [HttpGet(ApiRoutes.ProductAlias.Count)]
        public async Task<IActionResult> GetProductCount()
        {
            var productInfo = await _summaryService.GetInformationByNameAsync("product") as ProductInfoDto;
            
            return Ok(productInfo.TotalProducts);
        }
    }
}