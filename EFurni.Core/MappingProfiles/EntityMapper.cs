#pragma warning disable 8629
using System;
using AutoMapper;
using EFurni.Shared.DTOs;
using EFurni.Shared.Models;

namespace EFurni.Core.MappingProfiles
{
    public class EntityMapper : Profile
    {
        public EntityMapper()
        {
            CreateMap<ProductDto, Product>()
                .ForMember(dest => dest.Category,
                    opt => opt.MapFrom(src => new Category {CategoryName = src.CategoryName}))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => new Brand {BrandName = src.BrandName}))
                .ForMember(dest => dest.ProductSalesStatistic, opt => opt.MapFrom(src => new ProductSalesStatistic
                {
                    DateAdded = (DateTime) src.DateAdded,
                    NumberSold = src.NumberSold,
                    NumberViewed = src.NumberViewed
                }))
                .ForMember(dest => dest.BrandId, opt => opt.Ignore())
                .ForMember(dest => dest.CategoryId, opt => opt.Ignore())
                .ForMember(dest => dest.CustomerOrderItem, opt => opt.Ignore())
                .ForMember(dest => dest.Stock, opt => opt.Ignore())
                .ForMember(dest => dest.BasketItem, opt => opt.Ignore())
                .ForMember(dest => dest.Listed, opt => opt.Ignore())
                .ForMember(dest => dest.CustomerReview, opt => opt.Ignore());
                
        }
    }
}