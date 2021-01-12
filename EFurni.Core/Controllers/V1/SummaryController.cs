using System.Collections.Generic;
using System.Threading.Tasks;
using EFurni.Contract.V1;
using EFurni.Contract.V1.Responses;
using EFurni.Services;
using EFurni.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EFurni.Core.Controllers.V1
{
    [ApiController]
    public class SummaryController : ControllerBase
    {
        private readonly ISummaryService _summaryService;
        
        public SummaryController(ISummaryService summaryService)
        {
            _summaryService = summaryService;
        }
        
        [HttpGet(ApiRoutes.Summary.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var infos = await _summaryService.GetAllInformationAsync();

            return Ok(new Response<IEnumerable<Info>>(infos));
        }

        [HttpGet(ApiRoutes.Summary.Get)]
        public async Task<IActionResult> Get(string infoName)
        {
            var info = await _summaryService.GetInformationByNameAsync(infoName);

            if (info == null)
            {
                return NotFound("The specified info was not found.");
            }
            
            return Ok(new Response<Info>(info));
        }
    }
}