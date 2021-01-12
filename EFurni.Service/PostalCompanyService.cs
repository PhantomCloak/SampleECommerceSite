using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EFurni.Contract.V1.Queries.Create;
using EFurni.Contract.V1.Queries.Filter;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Infrastructure.Queries;
using EFurni.Infrastructure.Repositories;
using EFurni.Services;
using EFurni.Shared.DTOs;

namespace EFurni.Services
{
    internal class PostalCompanyService : IPostalCompanyService
    {
        private readonly IPostalServiceRepository<PostalCompanyFilterParams> _postalServiceRepository;
        private readonly ILocationQuery _locationQuery;
        private readonly IMapper _mapper;
        
        public PostalCompanyService(IPostalServiceRepository<PostalCompanyFilterParams> postalServiceRepository, IMapper mapper, ILocationQuery locationQuery)
        {
            _postalServiceRepository = postalServiceRepository;
            _mapper = mapper;
            _locationQuery = locationQuery;
        }
        
        public async Task<IEnumerable<PostalServiceDto>> GetAllPostalCompaniesAsync(
            PostalCompanyFilterParams postalCompanyFilterParams = null)
        {
            var postalServices=  await _postalServiceRepository.GetAllPostalServicesAsync(postalCompanyFilterParams);

            return _mapper.Map<IEnumerable<PostalServiceDto>>(postalServices);
        }

        public async Task<PostalServiceDto> GetPostalCompanyAsync(string companyName)
        {
            var postalService = await _postalServiceRepository.GetPostalServiceByNameAsync(companyName);

            return _mapper.Map<PostalServiceDto>(postalService);
        }

        public Task<PostalServiceDto> CreatePostalCompanyAsync(CreatePostalCompanyParams productParams)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> UpdatePostalCompanyAsync(PostalServiceDto reviewDto)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> DeletePostalCompanyAsync(int companyName)
        {
            throw new System.NotImplementedException();
        }
    }
}