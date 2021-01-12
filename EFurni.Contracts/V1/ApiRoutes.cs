namespace EFurni.Contract.V1
{
    public static class ApiRoutes
    {
        private const string Root = "api";

        private const string Version = "v1";

        private const string Base = Root + "/" + Version;

        public static class Authentication
        {
            public const string Login = Base + "/login";
            public const string Register = Base + "/register";
            public const string Logout = Base + "/logout";
        }

        public static class BasketItems
        {
            public const string GetAll = Base + "/basket";
            public const string Get = Base + "/basket/{productId}";
            public const string Create = Base + "/basket";
            public const string Update = Base + "/basket/{productId}";
            public const string Delete = Base + "/basket/{productId}";
        }

        public static class Product
        {
            public const string GetAll = Base + "/products";
            public const string Get = Base + "/products/{productId}";
            public const string Create = Base + "/products";
            public const string Update = Base + "/products/{productId}";
            public const string Delete = Base + "/products/{productId}";
        }

        public static class ProductAlias
        {
            public const string Count = Base + "/count";
        }
        
        public static class ProductReview
        {
            public const string GetAll = Base + "/products/{productId}/reviews";
            public const string Get = Base + "/products/{productId}/reviews/{reviewId}";
            public const string Create = Base + "/products/{productId}/reviews";
            public const string Update = Base + "/products/{productId}/reviews/{productId}";
            public const string Delete = Base + "/products/{productId}/reviews/{productId}";
        }
        
        public static class Brand
        {
            public const string GetAll = Base + "/brands";
            public const string Get = Base + "/brands/{brandName}";
            public const string Create = Base + "/brands";
            public const string Update = Base + "/brands/{brandName}";
            public const string Delete = Base + "/brands/{brandName}";
        }

        public static class BrandProduct
        {
            public const string GetAll = Base + "/brands/{brandName}/products";
            public const string Get = Base + "/brands/{brandName}/products/{productIndex}";
            public const string Create = Base + "/brands/{brandName}/products";
            public const string Update = Base + "/brands/{brandName}/products/{productIndex}";
            public const string Delete = Base + "/brands/{brandName}/products/{productIndex}";
        }

        public static class Category
        {
            public const string GetAll = Base + "/categories";
            public const string Get = Base + "/categories/{categoryName}";
            public const string Create = Base + "/categories";
            public const string Update = Base + "/categories/{categoryName}";
            public const string Delete = Base + "/categories/{categoryName}";
        }

        public static class Order
        {
            public const string GetAll = Base + "/orders";
            public const string Get = Base + "/orders/{orderId}";
            public const string Create = Base + "/orders";
            public const string Update = Base + "/orders/{orderId}";
            public const string Delete = Base + "/orders/{orderId}";
        }

        public static class Location
        {
            public const string GetCountries = Base + "/locations";
            public const string GetProvince = Base + "/locations/{countryTag}";
            public const string GetDistrict = Base + "/locations/{countryTag}/{provinceName}";
            public const string GetNeighborhood = Base + "/locations/{countryTag}/{provinceName}/{districtName}";
        }

        public static class Store
        {
            public const string GetAll = Base + "/stores";
            public const string Get = Base + "/stores/{storeId}";
            public const string Create = Base + "/stores";
            public const string Update = Base + "/stores/{storeId}";
            public const string Delete = Base + "/stores/{storeId}";
        }

        public static class StoreAlias
        {
            public const string StoreMatch = Base + "/stores/match";
        }

        public static class PostalService
        {
            public const string GetAll = Base + "/postaloffices";
            public const string Get = Base + "/postaloffices/{officeName}";
            public const string Create = Base + "/postaloffices";
            public const string Update = Base + "/postaloffices/{officeName}";
            public const string Delete = Base + "/postaloffices/{officeName}";
        }

        public static class Customer
        {
            public const string GetAll = Base + "/customers";
            public const string Get = Base + "/customers/{customerId}";
            public const string Create = Base + "/customers";
            public const string Update = Base + "/customers/{customerId}";
            public const string Delete = Base + "/customers/{customerId}";
        }

        public static class Summary
        {
            public const string GetAll = Base + "/infos";
            public const string Get = Base + "/info/{infoName}";
        }

        public static class CustomerAlias
        {
            public const string GetSelf = Base + "/customers/self";
        }
    }
}