using Microsoft.Extensions.Configuration;
using RestSharp;
using RestSharp.Serializers.Newtonsoft.Json;

namespace EFurni.Presentation.RestClientExtensions
{
    public class RestClientManager
    {
        private readonly string _backendAddress; 
        public RestClientManager(IConfiguration configuration)
        {
            _backendAddress = configuration["ApiServer:Address"];
        }   
        
        public RestClient GetRestClient()
        {
            var client = new RestClient(_backendAddress);
            
            // Override with Newtonsoft JSON Handler
            client.AddHandler("application/json", () => NewtonsoftJsonSerializer.Default);
            client.AddHandler("text/json", () => NewtonsoftJsonSerializer.Default);
            client.AddHandler("text/x-json", () => NewtonsoftJsonSerializer.Default);
            client.AddHandler("text/javascript", () => NewtonsoftJsonSerializer.Default);
            client.AddHandler("*+json", () => NewtonsoftJsonSerializer.Default);

            return client;
        }
        
    }        
}