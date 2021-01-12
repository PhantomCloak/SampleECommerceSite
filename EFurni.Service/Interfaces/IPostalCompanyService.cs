using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EFurni.Contract.V1.Queries.Create;
using EFurni.Contract.V1.Queries.Filter;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Shared.DTOs;

namespace EFurni.Services
{
    public interface IPostalCompanyService
    {
        Task<IEnumerable<PostalServiceDto>> GetAllPostalCompaniesAsync(PostalCompanyFilterParams postalCompanyFilterParams = null);
        Task<PostalServiceDto> GetPostalCompanyAsync(string companyName);
        Task<PostalServiceDto> CreatePostalCompanyAsync(CreatePostalCompanyParams productParams);
        Task<bool> UpdatePostalCompanyAsync(PostalServiceDto reviewDto);
        Task<bool> DeletePostalCompanyAsync(int companyName);
    }
}