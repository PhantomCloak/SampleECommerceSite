using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using EFurni.Contract.V1;
using EFurni.Contract.V1.Queries.Create;
using EFurni.Contract.V1.Queries.Filter;
using EFurni.Contract.V1.Responses;
using EFurni.Services;
using EFurni.Shared.DTOs;
using EFurni.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EFurni.Core.Controllers.V1
{
    [ApiController]
    public class ProductReviewController : ControllerBase
    {
        private readonly IProductReviewService _productReviewService;
        private readonly IMapper _mapper;
        
        public ProductReviewController(IProductReviewService productReviewService, IMapper mapper)
        {
            _productReviewService = productReviewService;
            _mapper = mapper;
        }
        
        [AllowAnonymous]
        [HttpGet(ApiRoutes.ProductReview.GetAll)]
        public async Task<IActionResult> GetReviews(int productId)
        {
            var reviews = await _productReviewService.GetReviewsAsync(productId);
            
            return Ok(new Response<IEnumerable<CustomerReviewDto>>(reviews));
        }

        [Authorize(Roles = "Trusted")]
        [HttpPost(ApiRoutes.ProductReview.Create)]
        public async Task<IActionResult> CreateReview(int productId,[FromQuery] CreateProductReviewQuery createQuery)
        {
            int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value,out var senderAccountId);

            var createProductParams = _mapper.Map<CreateProductReviewParams>(createQuery);
            createProductParams.AuthorAccountId = senderAccountId;
            createProductParams.ProductId = productId;
            
            var createdReview = await _productReviewService.CreateReviewAsync(createProductParams);

            if (createdReview == null) 
            {
                return BadRequest();
            }

            return Ok();
        }
        
    }
}