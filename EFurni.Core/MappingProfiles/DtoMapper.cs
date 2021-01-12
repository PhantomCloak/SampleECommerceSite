using AutoMapper;
using EFurni.Shared.DTOs;
using EFurni.Shared.Models;

namespace EFurni.Core.MappingProfiles
{
    public class DtoMapper : Profile
    {
        public DtoMapper()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.BrandName))
                .ForMember(dest => dest.ProductRating,
                    opt => opt.MapFrom(src => src.ProductSalesStatistic.ProductRating))
                .ForMember(dest => dest.DateAdded, opt => opt.MapFrom(src => src.ProductSalesStatistic.DateAdded))
                .ForMember(dest => dest.NumberViewed, opt => opt.MapFrom(src => src.ProductSalesStatistic.NumberViewed))
                .ForMember(dest => dest.NumberSold, opt => opt.MapFrom(src => src.ProductSalesStatistic.NumberSold));

            CreateMap<Brand, BrandDto>()
                .ForMember(dest => dest.TotalProducts, opt => opt.MapFrom(src => src.Product.Count));

            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Product));

            CreateMap<Store, StoreDto>()
                .ForMember(dest => dest.CountryTag, opt => opt.MapFrom(src => src.StoreAddress.CountryTag))
                .ForMember(dest => dest.Province, opt => opt.MapFrom(src => src.StoreAddress.Province))
                .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.StoreAddress.District))
                .ForMember(dest => dest.Neighborhood, opt => opt.MapFrom(src => src.StoreAddress.Neighborhood))
                .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.StoreAddress.ZipCode))
                .ForMember(dest => dest.AddressTextPrimary,
                    opt => opt.MapFrom(src => src.StoreAddress.AddressTextPrimary))
                .ForMember(dest => dest.AddressTextSecondary,
                    opt => opt.MapFrom(src => src.StoreAddress.AddressTextSecondary));

            CreateMap<CustomerOrder, CustomerOrderDto>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.ReceiverFirst + " " + src.ReceiverLast))
                .ForMember(dest => dest.SubOrders, opt => opt.MapFrom(src => src.CustomerOrderItem))
                .ForMember(dest => dest.ReceiverFirstName, opt => opt.MapFrom(src => src.ReceiverFirst))
                .ForMember(dest => dest.ReceiverLastName, opt => opt.MapFrom(src => src.ReceiverLast))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.OptionalEmail, opt => opt.MapFrom(src => src.OptionalMail))
                .ForMember(dest => dest.CargoCompany, opt => opt.MapFrom(src => src.PostalService.Postalservicename))
                .ForMember(dest => dest.OrderAddress, opt => opt.MapFrom(src => src.CustomerOrderAddress.AddressTextPrimary))
                .ForMember(dest => dest.ShippingPostalCode, opt => opt.MapFrom(src => src.CustomerOrderAddress.DestinationZipCode))
                .ForMember(dest => dest.CountryTag, opt => opt.MapFrom(src => src.CustomerOrderAddress.CountryTag))
                .ForMember(dest => dest.ProvinceName, opt => opt.MapFrom(src => src.CustomerOrderAddress.Province))
                .ForMember(dest => dest.DistrictName, opt => opt.MapFrom(src => src.CustomerOrderAddress.District))
                .ForMember(dest => dest.NeighborhoodName, opt => opt.MapFrom(src => src.CustomerOrderAddress.Neighborhood))
                .ForMember(dest => dest.CargoCost, opt => opt.MapFrom(src => src.CargoPrice))
                .ForMember(dest => dest.SubOrders, opt => opt.MapFrom(src => src.CustomerOrderItem));

            CreateMap<PostalService, PostalServiceDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ServiceId))
                .ForMember(dest => dest.ServicePrice, opt => opt.MapFrom(src => src.Price));

            CreateMap<CustomerReview, CustomerReviewDto>()
                .ForMember(dest => dest.CustomerFirstName, opt => opt.MapFrom(src => src.Customer.FirstName))
                .ForMember(dest => dest.CustomerLastName, opt => opt.MapFrom(src => src.Customer.LastName))
                .ForMember(dest => dest.CustomerPicture, opt => opt.MapFrom(src => src.Customer.ProfilePictureUrl));

            CreateMap<CustomerOrderItem, CustomerOrderItemDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.StoreName, opt => opt.MapFrom(src => src.Store.StoreName));
            
            CreateMap<Customer, CustomerDto>();

            CreateMap<Stock, StockDto>();
        }
    }
}