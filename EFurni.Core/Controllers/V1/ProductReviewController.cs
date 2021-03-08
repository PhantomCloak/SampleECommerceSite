using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using EFurni.Contract.V1;
using EFurni.Contract.V1.Queries.Create;
using EFurni.Contract.V1.Queries.Filter;
using EFurni.Contract.V1.Responses;
using EFurni.Core.AuthenticationExtension;
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
        private readonly IAuthenticationContext _authenticationContext;
        
        public ProductReviewController(IProductReviewService productReviewService, IMapper mapper, IAuthenticationContext authenticationContext)
        {
            _productReviewService = productReviewService;
            _mapper = mapper;
            _authenticationContext = authenticationContext;
            
            _authenticationContext.AttachCurrentContext(this);
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
            var senderAccountId = await _authenticationContext.SenderActorId();
            
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