using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.DTOs;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductShop
{
    public class StartUp
    {
        private static IMapper mapper;

        public static void Main(string[] args)
        {
            ProductShopContext context = new ProductShopContext();
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            ////Task 1
            // string usersJson = File.ReadAllText("../../../datasets/users.json");
            //Console.WriteLine(ImportUsers(context, usersJson));
            ////Task 2
            //string productsJson = File.ReadAllText("../../../datasets/products.json");
            //Console.WriteLine(ImportProducts(context, productsJson));
            ////Task 3
            //string categoriesJson = File.ReadAllText("../../../datasets/categories.json");
            //Console.WriteLine(ImportCategories(context, categoriesJson));
            ////Task 4
            //string categoryProductJson = File.ReadAllText("../../../datasets/categories-products.json");
            //Console.WriteLine(ImportCategoryProducts(context, categoryProductJson));
            ////Task 5
            //Console.WriteLine(GetProductsInRange(context));
            ////Task 6
            //Console.WriteLine(GetSoldProducts(context));
            ////Task 7
            //Console.WriteLine(GetCategoriesByProductsCount(context));
            ////Task 8
            //Console.WriteLine(GetUsersWithProducts(context));
        }

        //Task 8

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context
                .Users
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .OrderByDescending(u => u.ProductsSold.Count(p => p.Buyer != null))
                .Select(u => new
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProducts = new
                    {
                        Count = u.ProductsSold
                        .Where(p => p.Buyer != null)
                        .Count(p => p.Buyer != null),

                        Products = u.ProductsSold
                        .Where(p => p.Buyer != null)
                        .Select(p => new
                        {
                            p.Name,
                            p.Price
                        })
                    }
                })
                .ToList();

            DefaultContractResolver resolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                ContractResolver = resolver,
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            };

            var usersDto = new
            {
                Count = users.Count(),
                Users = users
            };

            string usersJson = JsonConvert.SerializeObject(usersDto, settings);
            return usersJson;
        }

        //Task 7
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context
                .Categories
                .OrderByDescending(c => c.CategoryProducts.Count)
                .Select(c => new
                {
                    Category = c.Name,
                    ProductsCount = c.CategoryProducts.Count(),
                    AveragePrice = $"{c.CategoryProducts.Average(cp => cp.Product.Price):F2}",
                    TotalRevenue = $"{c.CategoryProducts.Sum(cp => cp.Product.Price):F2}"
                })
                .ToArray();

            DefaultContractResolver resolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                ContractResolver = resolver,
                Formatting = Formatting.Indented
            };

            string categoriesJson = JsonConvert.SerializeObject(categories, settings);
            return categoriesJson;
        }

        //Task 6
        public static string GetSoldProducts(ProductShopContext context)
        {
            var products = context
                .Users
                .Where(u => u.ProductsSold.Count() >= 1 && u.ProductsSold.Any(p=> p.Buyer != null))
                .Select(u =>  new
                {
                    u.FirstName,
                    u.LastName,
                    SoldProducts = u.ProductsSold
                    .Select(p => new
                    {
                        p.Name,
                        p.Price,
                        BuyerFirstName = p.Buyer.FirstName,
                        BuyerLastName = p.Buyer.LastName
                    })
                })
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ToArray();

            DefaultContractResolver resolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                ContractResolver = resolver,
                Formatting = Formatting.Indented
            };

            string soldProductsJson = JsonConvert.SerializeObject(products, settings);
            return soldProductsJson;
        }

        //Task 5
        public static string GetProductsInRange(ProductShopContext context)
        {
            ProductOutputDto[] products = context
                .Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .Select(p => new ProductOutputDto
                {
                    Name = p.Name,
                    Price = p.Price,
                    Seller = $"{p.Seller.FirstName} {p.Seller.LastName}"
                })
                .OrderBy(p => p.Price)
                .ToArray();

            DefaultContractResolver resolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = resolver
            };

            string productsJson = JsonConvert.SerializeObject(products, settings);
            return productsJson;
        }

        //Task 4
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            ICollection<CategoryProductDto> categoryProducts = JsonConvert.DeserializeObject<ICollection<CategoryProductDto>>(inputJson);

            InitializeMapper();

            ICollection<CategoryProduct> mappedCategoryProducts = mapper.Map<ICollection<CategoryProduct>>(categoryProducts);

            context.CategoryProducts.AddRange(mappedCategoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count}";
        }

        //Task 3
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            IEnumerable<CategoryDto> categories = JsonConvert.DeserializeObject<ICollection<CategoryDto>>(inputJson)
                .Where(c => c.Name != null);

            InitializeMapper();

            IEnumerable<Category> mappedCategories = mapper.Map<ICollection<Category>>(categories);

            context.Categories.AddRange(mappedCategories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count()}";
        }

        //Task 2
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            ICollection<ProductInputDto> products = JsonConvert.DeserializeObject<ICollection<ProductInputDto>>(inputJson);

            InitializeMapper();

            ICollection<Product> mappedProducts = mapper.Map<ICollection<Product>>(products);

            context.Products.AddRange(mappedProducts);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }

        //Task 1
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            ICollection<UserInputDto> users = JsonConvert.DeserializeObject<ICollection<UserInputDto>>(inputJson);

            InitializeMapper();

            ICollection<User> mappedUsers = mapper.Map<ICollection<User>>(users);

            context.Users.AddRange(mappedUsers);
            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }

        private static void InitializeMapper()
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.AddProfile<ProductShopProfile>());
            mapper = new Mapper(config);
        }
    }
}