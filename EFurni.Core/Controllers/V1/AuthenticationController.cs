using System.Threading.Tasks;
using AutoMapper;
using EFurni.Contract.V1;
using EFurni.Contract.V1.Queries;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Contract.V1.Queries.Validation;
using EFurni.Contract.V1.Responses;
using EFurni.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EFurni.Core.Controllers.V1
{
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;

        public AuthenticationController(
            IAuthenticationService authenticationService,
            IMapper mapper)
        {
            _authenticationService = authenticationService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost(ApiRoutes.Authentication.Login)]
        public async Task<IActionResult> Login([FromQuery] LoginQuery loginQuery)
        {
            var token = await _authenticationService.LoginAsync(loginQuery.Username,loginQuery.Password);

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new Response<string>("The specified account does not exist."));
            }

            return Ok(new Response<string>(token));
        }

        [HttpPost(ApiRoutes.Authentication.Register)]
        public async Task<IActionResult> Register([FromQuery] RegisterUserQuery registerQuery)
        {
            var userQuery = _mapper.Map<RegisterUserQuery, RegisterUserParams>(registerQuery);

            bool result = await _authenticationService.RegisterUserAsync(userQuery);

            if (!result)
            {
                return BadRequest(new Response<string>("Registration failed."));
            }

            return Ok(new Response<string>("Registration completed successfully"));
        }

        [HttpPost(ApiRoutes.Authentication.Logout)]
        public async Task<IActionResult> Logout(string token)
        {
            return NotFound();
        }
    }
}