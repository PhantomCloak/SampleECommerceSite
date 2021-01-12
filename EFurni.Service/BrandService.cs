using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EFurni.Contract.V1.Queries.Create;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Infrastructure.Repositories;
using EFurni.Shared.DTOs;
using EFurni.Shared.Models;

namespace EFurni.Services
{
    internal class BrandService : IBrandService
    {
        private readonly IBrandRepository<BrandFilterParams> _brandRepository;
        private readonly IMapper _mapper;
        
        public BrandService(IBrandRepository<BrandFilterParams> brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<BrandDto>> GetAllBrandsAsync(BrandFilterParams brandFilterParams = null, PaginationParams paginationParams = null)
        {
            var brands = await _brandRepository.GetAllBrandsAsync(brandFilterParams, paginationParams);

            return _mapper.Map<IEnumerable<BrandDto>>(brands);
        }

        public async Task<BrandDto> GetBrandAsync(string brandName)
        {
            var brand = await _brandRepository.GetBrandByNameAsync(brandName);

            return _mapper.Map<BrandDto>(brand);
        }

        public async Task<BrandDto> CreateBrandAsync(CreateBrandParams createBrandParams)
        {
            var newBrand = new Brand
            {
                BrandName = createBrandParams.BrandName
            };
            
            await _brandRepository.CreateBrandAsync(newBrand);

            return _mapper.Map<BrandDto>(newBrand);
        }

        public async Task<bool> DeleteBrandAsync(string brandName)
        {
            return await _brandRepository.DeleteBrandAsync(brandName);
        }
    }
}