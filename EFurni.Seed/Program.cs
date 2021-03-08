using System;
using System.Collections.Generic;
using System.Data;
using Bogus;
using EFurni.Seed.Seeders.SeederShared;
using EFurni.Shared.Models;

// ReSharper disable All
//Don't use that class
namespace EFurni.Seed
{
    internal class Program
    {
        private const int productCount = 75;
        private const int customerCount = 6;
        private const int storeCount = 4;
        private const int brandCount = 5;
        private const int categoryCount = 3;


        //const variables
        private static string PhotoServer = "/img/";

        static readonly string[] CategoryNames = new string[]
        {
            "sofa",
            "chair",
            "table",
        };

        static string[] ColorNames = new string[]
        {
            "red",
            "green",
            "blue",
        };

        private static Dictionary<string, string[]> SubCategories = new Dictionary<string, string[]>()
        {
            {CategoryNames[0], new string[] {"Cabriole", "Sette", "Lawson"}},
            {CategoryNames[1], new string[] {"Slipper", "Tub", "Club"}},
            {CategoryNames[2], new string[] {"Office", "Kitchen", "Computer"}},
        };


        private const string connectionString =
            "server=localhost;database=efurni;uid=service;pwd=badfood11;convert zero datetime=True;";

        private enum Color : int
        {
            Red = 1,
            Green = 2,
            Blue = 3
        }

        private static void Seed()
        {
            // var dbConnection =
                // new MySqlConnection(connectionString);
            // dbConnection.Open();
            // Seeder.DbConnection = dbConnection;
            
            // if (dbConnection.State != ConnectionState.Open)
            // {
            //     return;
            // }
            Seeder.WipeTable(typeof(StoreAddress));
            Seeder.WipeTable(typeof(Stock));
            Seeder.WipeTable(typeof(Store));
            Seeder.WipeTable(typeof(CustomerReview));
            Seeder.WipeTable(typeof(ProductSalesStatistic));
            Seeder.WipeTable(typeof(Product));
            Seeder.WipeTable(typeof(Brand));
            Seeder.WipeTable(typeof(Category));
            Seeder.WipeTable(typeof(Customer));
            Seeder.WipeTable(typeof(Account));


            Seeder.Seed<Account>(() =>
            {
                int customerCounter = 1;

                var fakeAccounts = new Faker<Account>()
                    .RuleFor(a => a.AccountId, f => customerCounter++)
                    .RuleFor(a => a.Email, f => f.Person.Email)
                    .RuleFor(a => a.Password, SeederHelper.CreateMd5(SeederHelper.RandomString(16)))
                    .RuleFor(a=>a.DeletedAt,f=> DateTime.Now)
                    .RuleFor(a=>a.Deleted,f=> false);

                return fakeAccounts;
            }, customerCount);
            Seeder.Seed<Customer>(() =>
            {
                int customerCounter = 1;

                var fakeCustomers = new Faker<Customer>()
                    .RuleFor(u => u.CustomerId, f => customerCounter)
                    .RuleFor(u => u.AccountId, f => customerCounter++)
                    .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                    .RuleFor(u => u.LastName, f => f.Name.LastName())
                    .RuleFor(u => u.ProfilePictureUrl, f => f.Internet.Avatar())
                    .RuleFor(u => u.PhoneNumber, f => f.Random.Int(1000, 2000).ToString());

                return fakeCustomers;
            }, customerCount);
            Seeder.Seed<Brand>(() =>
            {
                int brandCounter = 1;

                var fakeBrands = new Faker<Brand>()
                    .RuleFor(b => b.BrandId, f => brandCounter++)
                    .RuleFor(b => b.BrandName, f => f.Company.CompanyName());

                return fakeBrands;
            }, brandCount);
            Seeder.Seed<Category>(() =>
            {
                int categoryCounter = 1;

                var fakeCategories = new Faker<Category>()
                    .RuleFor(c => c.CategoryName, f =>CategoryNames[categoryCounter -1])
                    .RuleFor(c => c.CategoryId, f => categoryCounter++);

                return fakeCategories;
            }, categoryCount);
            
            // Seeder.Seed<CustomerAddress>(() =>
            // {
            //     int customerCounter = 1;
            //     var fakeCustomerAddresses = new Faker<CustomerAddress>()
            //         .RuleFor(u => u.CustomerId, f => customerCounter++)
            //         .RuleFor(u => u.Country, f => f.Address.Country())
            //         .RuleFor(u => u.Province, f => f.Address.City())
            //         .RuleFor(u => u.District, f => f.Address.City())
            //         .RuleFor(u => u.AddressTextPrimary, f => f.Address.FullAddress())
            //         .RuleFor(u => u.AddressTextSecondary, f => f.Address.FullAddress())
            //         .RuleFor(s => s.ZipCode, f => f.PickRandom(new []{"20000","34000","06090","01010","07010"}));
            //     return fakeCustomerAddresses;
            // }, customerCount);
            //
            Seeder.Seed<Store>(() =>
            {
                int storeCounter = 1;

                var fakeStores = new Faker<Store>()
                    .RuleFor(s => s.StoreId, f => storeCounter++)
                    .RuleFor(s => s.Email, f => f.Person.Email)
                    .RuleFor(s => s.PhoneNumber, f => f.Person.Phone)
                    .RuleFor(s => s.StoreName, f => f.Company.CompanyName());

                return fakeStores;
            }, storeCount);
            Seeder.Seed<StoreAddress>(() =>
            {
                int storeCounter = 1;
            
                var fakeStoreAddresses = new Faker<StoreAddress>()
                    .RuleFor(s => s.StoreId, f => storeCounter++)
                    .RuleFor(s => s.CountryTag, f =>"TR")
                    .RuleFor(s => s.Province, f => f.Address.City())
                    .RuleFor(s => s.District, f => f.Address.City())
                    .RuleFor(s => s.Neighborhood, f => f.Address.StreetAddress())
                    .RuleFor(s => s.ZipCode, f => f.PickRandom(new []{"20000","34000","06090","01010","07010"}))
                    .RuleFor(s => s.AddressTextPrimary, f => f.Address.FullAddress())
                    .RuleFor(s => s.AddressTextSecondary, f => "");

                return fakeStoreAddresses;
            }, storeCount);
            Seeder.Seed<Product>(() =>
            {
                string[] productNames =
                {
                    new Faker().Music.Genre() + " Chair",
                    new Faker().Music.Genre() + " Armchair",
                    new Faker().Music.Genre() + " Deck chair",
                    new Faker().Music.Genre() + " Cantilever Chair",
                    new Faker().Music.Genre() + " Deck",
                    new Faker().Music.Genre() + " Desk",
                    new Faker().Music.Genre() + " Stool",
                    new Faker().Music.Genre() + " Sofa",
                    new Faker().Music.Genre() + " Sofa",
                    new Faker().Music.Genre() + " Sofa",
                    new Faker().Music.Genre() + " Sofa",
                };

                int productCounter = 1;

                var fakeProducts = new Faker<Product>()
                    .RuleFor(p => p.ProductColor, f => ColorNames[f.Random.Int(0, ColorNames.Length - 1)])
                    .RuleFor(p => p.CategoryId, f => f.Random.Int(1, CategoryNames.Length))
                    .RuleFor(p => p.ProductId, f => productCounter++)
                    .RuleFor(p => p.ProductName,
                        (f, u) => $"{u.ProductColor} {f.Lorem.Word()} {CategoryNames[u.CategoryId - 1]}")
                    .RuleFor(p => p.SubType, (f, u) => f.PickRandom(SubCategories[CategoryNames[u.CategoryId - 1]]))
                    .RuleFor(p => p.ProductImage, (f, u) => $"{PhotoServer}{u.ProductColor}-{CategoryNames[u.CategoryId - 1]}-{1}.jpg")
                    .RuleFor(p => p.ProductWidth, f => f.Random.Float(50, 120))
                    .RuleFor(p => p.ProductHeight, f => f.Random.Float(50, 120))
                    .RuleFor(p => p.ProductWeight, f => f.Random.Int(15, 100))
                    .RuleFor(p => p.BoxPieces, f => f.Random.Int(20, 50))
                    .RuleFor(p => p.BrandId, f => f.Random.Int(1, brandCount))
                    .RuleFor(p => p.ModelYear, f => f.Random.Int(2000, 2020))
                    .RuleFor(p => p.ListPrice, f => f.Random.Double(25, 150))
                    .RuleFor(p => p.Discount, f => f.PickRandom(0, 0, 0, 0, 0, 0, 0, 5, 10, 20))
                    .RuleFor(p => p.Description, f => f.Commerce.ProductDescription());
                return fakeProducts;
            }, productCount);
            Seeder.Seed<Stock>(() =>
            {
                int productCounter = 1;

                var fakeStoreStocks = new Faker<Stock>()
                    .RuleFor(s => s.StockId, f => productCounter)
                    .RuleFor(s => s.ProductId, f => productCounter++)
                    .RuleFor(s => s.StoreId, f => f.Random.Int(1, 2))
                    .RuleFor(s => s.Quantity, f => f.Random.Int(50, 120));

                return fakeStoreStocks;
            }, productCount);

            Seeder.Seed<CustomerReview>(() =>
            {
                var fakeComments = new Faker<CustomerReview>()
                    .RuleFor(s => s.CustomerId, f => f.Random.Int(1, customerCount))
                    .RuleFor(s => s.ProductId, f => f.Random.Int(1, productCount))
                    .RuleFor(s => s.CustomerComment, f => f.Lorem.Paragraph(4))
                    .RuleFor(s => s.CustomerRating, f => f.Random.Float(0, 5))
                    .RuleFor(s => s.ReplyReviewId, f => 0);

                return fakeComments;
            }, productCount * 4);
            
            Seeder.Seed<PostalService>(() =>
            {
                int postalServiceCounter = 1;

                var fakeComments = new Faker<PostalService>()
                    .RuleFor(s => s.ServiceId, f => postalServiceCounter++)
                    // .RuleFor(s => s.PostalServiceName, f => f.Company.CompanyName())
                    .RuleFor(s => s.Price, f => f.Random.Double(25, 100));

                return fakeComments;
            }, storeCount);
            
            Seeder.Seed<ProductSalesStatistic>(() =>
            {
                int productCounter = 1;

                var productStatistic = new Faker<ProductSalesStatistic>()
                    .RuleFor(p => p.ProductId, f => productCounter++)
                    .RuleFor(p => p.NumberSold, f => f.Random.Int(20, 500))
                    .RuleFor(p => p.NumberViewed, f => f.Random.Int(100, 6000))
                    .RuleFor(p => p.DateAdded, f => SeederHelper.GetRandomDate(2019));

                return productStatistic;
            }, productCount);

        }

        private static void Main(string[] args)
        {
            Seed();
        }
    }
}