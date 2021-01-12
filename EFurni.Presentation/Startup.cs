using System;
using Blazored.LocalStorage;
using EFurni.Presentation.Clients;
using EFurni.Presentation.Clients.AuthenticationClient;
using EFurni.Presentation.Clients.ClientInterfaces;
using EFurni.Presentation.RestClientExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Radzen;
using RestSharp;
using Syncfusion.Blazor;


namespace EFurni.Presentation
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzQ5NTUyQDMxMzgyZTMzMmUzMGUxWG85NDk0cnlPRXZjQmFYcVNGNXVzVDB2RGNyVkhSRUpDR1FQdzZyZk09");
			
			services.AddRazorPages();
			services.AddServerSideBlazor();

			services.AddBlazoredLocalStorage();
			services.AddScoped<LocalStorageService>();
			services.AddScoped<NotificationService>();
			services.AddScoped<RestClientManager>();
			services.AddScoped<RestClient>();
			
			services.AddScoped<IAuthenticationClient,AuthenticationClient>();
			services.AddScoped<IPostalCodeClient,PostalCodeClient>();
			services.AddScoped<IStoreClient,StoreClient>();
			services.AddScoped<IBasketClient, BasketLocalClient>();
			services.AddScoped<IProductClient,ProductClient>();
			services.AddScoped<ISummaryClient,SummaryClient>();
			services.AddScoped<IOrderClient,OrderClient>();
			services.AddScoped<ICustomerClient, CustomerClient>();
			services.AddScoped<IReviewClient, ReviewClient>();
			services.AddScoped<IPostalServiceClient, PostalServiceClient>();
			
			
			services.AddSyncfusionBlazor(true);	
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			// if (env.IsDevelopment())
			// {
				app.UseDeveloperExceptionPage();
			// }
				// app.UseExceptionHandler("/Error");

			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapRazorPages();
				endpoints.MapBlazorHub();
			});
		}
	}
}
