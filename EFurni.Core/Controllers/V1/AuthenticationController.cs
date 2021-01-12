using System.Threading.Tasks;
using AutoMapper;
using EFurni.Contract.V1;
using EFurni.Contract.V1.Queries;
using EFurni.Contract.V1.Queries.QueryParams;
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
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AuthenticationController(
            IAuthenticationService authenticationService,
            ITokenService tokenService,
            IMapper mapper)
        {
            _authenticationService = authenticationService;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost(ApiRoutes.Authentication.Login)]
        public async Task<IActionResult> Login(string userName,string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                return Unauthorized(new Response<string>("The user name or password is incorrect."));
            }

            var (validated, validatedUser) = await _authenticationService.ValidateUser(userName,password);

            if (!validated)
            {
                return Unauthorized(new Response<string>("The specified account does not exist."));
            }

            string oldToken = await _tokenService.GetAccountTokenAsync(validatedUser);

            if (!string.IsNullOrEmpty(oldToken))
            {
                await _tokenService.DeleteTokenAsync(oldToken);
            }

            string token = await _tokenService.CreateTokenAsync(validatedUser);

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