using System;
using System.Net;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using EFurni.Contract.V1;
using EFurni.Contract.V1.Responses;
using EFurni.Presentation.Clients.ClientInterfaces;
using EFurni.Presentation.RestClientExtensions;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace EFurni.Presentation.Clients.AuthenticationClient
{
    public class AuthenticationClient : IAuthenticationClient
    {
        private readonly LocalStorageService _localStorageService;

        private readonly RestClient client;

        public async Task<bool> IsLoggedIn()
        {
            string str= await AuthenticationToken();
            return !string.IsNullOrEmpty(str);
        }

        public async Task<string> AuthenticationToken()
        {
            return await _localStorageService.GetItemAsStringAsync("token");
        }

        public AuthenticationClient(RestClientManager clientManager,LocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
            client = clientManager.GetRestClient();
        }
        
        public async Task<bool> Login(string email, string password)
        {
            var loginRequest = new RestRequest(ApiRoutes.Authentication.Login, Method.POST)
                .AddQueryParameter("userName", email)
                .AddQueryParameter("password", password);

            var response = await client.ExecuteAsync<Response<string>>(loginRequest);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine(response.Content);
                return false;
            }
            
            await _localStorageService.SetItemAsync("token",response.Data.Data);
            
            return true;
        }

        public async Task Logout()
        {
            await _localStorageService.RemoveItemAsync("token");
        }

        public async Task<(bool,string)> Register(string username,string password,string firstName,string lastName,string phone)
        {
            var loginRequest = new RestRequest(ApiRoutes.Authentication.Register, Method.POST)
                .AddQueryParameter("email", username)
                .AddQueryParameter("password", password)
                .AddQueryParameter("firstName", firstName)
                .AddQueryParameter("lastName", lastName)
                .AddQueryParameter("phoneNumber", phone);

            var response = await client.ExecuteAsync<Response<string>>(loginRequest);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return (false,response.Content);
            }
            
            return (true,response.Content);
        }
    }
}