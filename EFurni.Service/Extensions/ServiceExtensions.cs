using EFurni.Core.Services;
using EFurni.Infrastructure.OutputDevices;
using EFurni.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace EFurni.Service.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICryptographyService, CryptographyService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPostalCompanyService, PostalCompanyService>();
            services.AddScoped<IProductReviewService, ProductReviewService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IStoreService, StoreService>();
            services.AddScoped<IZipDistanceService, ZipDistanceService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<ISummaryService, SummaryService>();
            services.Decorate<ISummaryService, CachedSummaryCache>();

            services.AddScoped<IRepositoryQueryOutputDevice, RepositoryQueryOutputDeviceDevice>();
            
            services.AddSingleton<IUriGeneratorService>(provider =>
            {
                var accessor = provider.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext.Request;
                var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent(), request.Path.Value);
                return new UriGeneratorService(absoluteUri);
            });
        }
    }
}