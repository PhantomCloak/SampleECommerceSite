using System;
using System.IO;
using EFurni.Core.Authentication;
using EFurni.Core.AuthenticationExtension;
using EFurni.Service.OutputDevices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;

namespace EFurni.Core.Extensions
{
    public static class ServiceExtensions
    {
        private static IConfigurationRoot Configuration { get; set; }

        static ServiceExtensions()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
        }

        public static bool AddCacheServices(this IServiceCollection services)
        {
            try
            {
                services.AddMemoryCache();
                services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(Configuration["Redis:ConnectionString"]));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen((options) =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo(){Title = "EFurni.Core"}
                );
                
                options.AddSecurityDefinition("CustomAuthentication", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "CustomAuthentication",
                    In = ParameterLocation.Header,
                    Description = "Basic Authorization header."
                });
 
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "CustomAuthentication"
                            }
                        },
                        new string[] {}
                    }
                });
            });
        }

        public static void AddOutputDevices(this IServiceCollection services)
        {
            services.AddScoped<IProductServiceOutputDevice, ProductServiceOutputDeviceDevice>();
        }

        public static void AddAuthenticationContext(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationContext, AuthenticationContext>();
        }
        
    }
}