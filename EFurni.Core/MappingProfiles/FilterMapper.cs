using AutoMapper;
using EFurni.Contract.V1.Queries;
using EFurni.Contract.V1.Queries.Create;
using EFurni.Contract.V1.Queries.Filter;
using EFurni.Contract.V1.Queries.QueryParams;

namespace EFurni.Core.MappingProfiles
{
    public class FilterMapper : Profile
    {
        public FilterMapper()
        {
            CreateMap<PaginationQuery, PaginationParams>();
            CreateMap<RegisterUserQuery,RegisterUserParams>();
            CreateMap<ProductFilterQuery, ProductFilterParams>();
            CreateMap<BrandFilterQuery, BrandFilterParams>();
            CreateMap<CategoryFilterQuery, CategoryFilterParams>();
            CreateMap<CustomerFilterQuery, CustomerFilterParams>()
                .ForMember(dest => dest.AccountIds, opt => opt.Ignore());
            
            CreateMap<CreateOrderQuery, CreateOrderParams>()
                .ForMember(dest => dest.AccountId, opt => opt.Ignore())
                .ForMember(dest => dest.StoreName, opt => opt.Ignore());
            CreateMap<CreateBrandQuery, CreateBrandParams>();
            CreateMap<CreateCategoryQuery,CreateCategoryParams>();
            CreateMap<CreateProductReviewQuery, CreateProductReviewParams>()
                .ForMember(dest => dest.AuthorAccountId, opt => opt.Ignore())
                .ForMember(dest => dest.ProductId, opt => opt.Ignore());

        }
    }
}