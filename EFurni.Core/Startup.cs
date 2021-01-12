using AutoMapper;
using EFurni.Core.Extensions;
using EFurni.Core.Handlers;
using EFurni.Core.Options;
using EFurni.Infrastructure.Extensions;
using EFurni.Service.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace EFurni.Core
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.TypeNameHandling = TypeNameHandling.None);

            services.AddCacheServices();
            services.AddDatabaseContexts();
            services.AddSwagger();
            services.AddApplicationRepositories();
            services.AddApplicationServices();
            services.AddOutputDevices();
            services.AddAuthenticationContext();
            
            services.AddAutoMapper(typeof(Startup));
            
            services.AddAuthentication("CustomAuthentication")
                .AddScheme<AuthenticationSchemeOptions,TokenBasedAuthenticationHandler>("CustomAuthentication",null);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,IMapper mapper)
        {
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
            
            // if (env.IsDevelopment())
            // {
                app.UseDeveloperExceptionPage();
            // }

            SwaggerOptions swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);
            
            app.UseSwagger(options =>
            {
                options.RouteTemplate = swaggerOptions.JsonRoute;
            });
            
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(swaggerOptions.UIEndpoint,swaggerOptions.Description);
            });
            app.UseHsts();
            // app.UseHttpsRedirection();
            
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}