using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using EFurni.Contract.V1;
using EFurni.Contract.V1.Responses;
using EFurni.Core.AuthenticationExtension;
using EFurni.Services;
using EFurni.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EFurni.Core.Controllers.V1
{
    [ApiController]
    public class CustomerAliasController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IAuthenticationContext _authenticationContext;
        private readonly IMapper _mapper;
        
        public CustomerAliasController(ICustomerService customerService,IAuthenticationContext authenticationContext, IMapper mapper)
        {
            _customerService = customerService;
            _authenticationContext = authenticationContext;
            _mapper = mapper;
            
            _authenticationContext.AttachCurrentContext(this);
        }
        
        [Authorize(Roles ="Trusted")]
        [HttpGet(ApiRoutes.CustomerAlias.GetSelf)]
        public async Task<IActionResult> GetSelf()
        {
            var userId = await _authenticationContext.SenderActorId();

            var user = await _customerService.GetCustomerByAccountIdAsync(userId);
            return Ok(new Response<CustomerDto>(_mapper.Map<CustomerDto>(user)));
        }
    }
}