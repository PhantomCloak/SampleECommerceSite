using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EFurni.Contract.V1;
using EFurni.Contract.V1.Responses;
using EFurni.Services;
using EFurni.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EFurni.Core.Controllers.V1
{
    [ApiController]
    public class PostalCompanyController : ControllerBase
    {
        private readonly IPostalCompanyService _postalCompanyService;
        private readonly IMapper _mapper;
        
        public PostalCompanyController(IPostalCompanyService postalCompanyService, IMapper mapper)
        {
            _postalCompanyService = postalCompanyService;
            _mapper = mapper;
        }
        
        [HttpGet(ApiRoutes.PostalService.GetAll)]
        public async Task<IActionResult> GetAllPostOffices()
        {
            var result = await _postalCompanyService.GetAllPostalCompaniesAsync();
            var response = _mapper.Map<List<PostalServiceDto>>(result);

            return Ok(new Response<IEnumerable<PostalServiceDto>>(response));
        }
        
        [HttpGet(ApiRoutes.PostalService.Get)]
        public async Task<IActionResult> GetPostOffices(string officeName)
        {
            var result = await _postalCompanyService.GetPostalCompanyAsync(officeName);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(new Response<PostalServiceDto>(_mapper.Map<PostalServiceDto>(result)));
        }
        
        [HttpPost(ApiRoutes.PostalService.Create)]
        public void CreatePostOffice()
        {
            throw new NotImplementedException();
        }
        
        [HttpPut(ApiRoutes.PostalService.Update)]
        public void UpdatePostOffice()
        {
            throw new NotImplementedException();
        }
        
        [HttpDelete(ApiRoutes.PostalService.Delete)]
        public void DeletePostOffice(string officeName)
        {
            throw new NotImplementedException();
        }
    }
}