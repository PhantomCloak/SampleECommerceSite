using System.IO;
using EFurni.Contract.V1.Queries.Filter;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Infrastructure.Data;
using EFurni.Infrastructure.Queries;
using EFurni.Infrastructure.Repositories;
using EFurni.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EFurni.Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
        private static IConfiguration Configuration { get; set; }
        static ServiceExtensions()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
        }
        public static void AddDatabaseContexts(this IServiceCollection services)
        {
            var productionDbConnectionStr = Configuration["Database::ProductionDatabase"];
            var locationDbConnectionStr = Configuration["Database::LocationDatabase"];
            
            services.AddDbContext<EFurniContext>(opt => opt.UseNpgsql(productionDbConnectionStr));
            services.AddDbContext<LocationsContext>(opt=> opt.UseNpgsql(locationDbConnectionStr));
        }
        public static void AddApplicationRepositories(this IServiceCollection services)
        {
            services.AddScoped<ILocationQuery, LocationQuery>();
            
            services.AddScoped<IBasketRepository, BasketRepository>();

            services.AddScoped<IAccountRepository<AccountFilterParams>, AccountRepository>();
            services.Decorate<IAccountRepository<AccountFilterParams>, CachedAccountRepository>();
            
            services.AddScoped<IBrandRepository<BrandFilterParams>, BrandRepository>();
            
            services.AddScoped<ICategoryRepository<CategoryFilterParams>, CategoryRepository>();
            
            services.AddScoped<ICustomerRepository<CustomerFilterParams>, CustomerRepository>();
            services.Decorate<ICustomerRepository<CustomerFilterParams>, CachedCustomerRepository>();
            
            services.AddScoped<IOrderRepository<OrderFilterParams>, OrderRepository>();
            
            services.AddScoped<IPostalServiceRepository<PostalCompanyFilterParams>, PostalServiceRepository>();
            
            services.AddScoped<IProductRepository<ProductFilterParams>, ProductRepository>();
            services.Decorate<IProductRepository<ProductFilterParams>, CachedProductRepository>();
            
            services.AddScoped<IReviewRepository, ReviewRepository>();
            
            services.AddScoped<IStoreRepository<StoreFilterParams>,StoreRepository>();
            
            services.AddScoped<ITokenRepository,TokenRepository>();

            services.AddScoped<IDistributedCacheAdapter, DistributedCacheAdapter>();
        }
    }
}