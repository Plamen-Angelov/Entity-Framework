using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using AutoMapper;
using ProductShop.Data;
using ProductShop.Dto.Input;
using ProductShop.Dto.Output;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            ProductShopContext context = new ProductShopContext();
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            ////Problem 1
            //string inputUsers = File.ReadAllText("datasets/users.xml");
            //Console.WriteLine(ImportUsers(context, inputUsers));
            ////Problem 2
            //string inputProducts = File.ReadAllText("datasets/products.xml");
            //Console.WriteLine(ImportProducts(context, inputProducts));
            ////Problem 3
            //string inputCategories = File.ReadAllText("datasets/categories.xml");
            //Console.WriteLine(ImportCategories(context, inputCategories));
            ////Problem 4
            //string inputCategoryProducts = File.ReadAllText("datasets/categories-products.xml");
            //Console.WriteLine(ImportCategoryProducts(context, inputCategoryProducts));
            ////Problem 5
            //Console.WriteLine(GetProductsInRange(context));
            ////Problem 6
            //Console.WriteLine(GetSoldProducts(context));
            ////Problem 7
            //Console.WriteLine(GetCategoriesByProductsCount(context));
            ////Problem 8
            //Console.WriteLine(GetUsersWithProducts(context));
        }

        //Problem 8
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            UserWithProductsCountOutputDto[] dtos = context
                .Users
                .Where(u => u.ProductsSold.Any())
                .Select(u => new UserWithProductsCountOutputDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProducts = new ProductCountDto
                    {
                        CountOfSoldProducts = u.ProductsSold.Count(),
                        Products = u.ProductsSold
                        .Select(p => new ProductDto
                        {
                            Name = p.Name,
                            Price = p.Price
                        })
                        .OrderByDescending(p => p.Price)
                        .ToList()
                    }
                })
                .OrderByDescending(u => u.SoldProducts.CountOfSoldProducts)
                .Take(10)
                .ToArray();

            XmlRootAttribute root = new XmlRootAttribute("Users");
            XmlSerializer serializer = new XmlSerializer(typeof(UserWithProductsCountOutputDto[]), root);

            StringBuilder sb = new StringBuilder();
            using StringWriter writer = new StringWriter(sb);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            serializer.Serialize(writer, dtos, namespaces);

            return sb.ToString().TrimEnd();
        }

        //Problem 7
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            CategoryOutputDto[] dtos = context
                .Categories
                .Select(c => new CategoryOutputDto
                {
                    Name = c.Name,
                    NumberOfProducts = c.CategoryProducts.Count(),
                    AveragePrice = c.CategoryProducts.Average(cp => cp.Product.Price),
                    TotalRevenue = c.CategoryProducts.Sum(cp => cp.Product.Price)
                })
                .OrderByDescending(c => c.NumberOfProducts)
                .ThenBy(c => c.TotalRevenue)
                .ToArray();

            StringBuilder sb = new StringBuilder();
            XmlSerializer serializer = new XmlSerializer(typeof(CategoryOutputDto[]), new XmlRootAttribute("Categories"));

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            StringWriter writer = new StringWriter(sb);
            serializer.Serialize(writer, dtos, namespaces);

            return sb.ToString().TrimEnd();
        }

        //Problem 6
        public static string GetSoldProducts(ProductShopContext context)
        {
            UserOutputDto[] users = context
                .Users
                .Where(u => u.ProductsSold.Count >= 1)
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Select(u => new UserOutputDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    soldProducts = u.ProductsSold
                    .Select(p => new ProductOutputDto
                    {
                        Name = p.Name,
                        Price = p.Price
                    })
                    .ToList()
                })
                .Take(5)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            XmlRootAttribute root = new XmlRootAttribute("Users");
            XmlSerializer serializer = new XmlSerializer(typeof(UserOutputDto[]), root);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            using StringWriter writer = new StringWriter(sb);
            serializer.Serialize(writer, users, namespaces);

            return sb.ToString().TrimEnd();
        }

        //Problem 5
        public static string GetProductsInRange(ProductShopContext context)
        {
            ProductOutputDto[] dtos = context
                .Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .Select(p => new ProductOutputDto
                {
                    Name = p.Name,
                    Price = p.Price,
                    BuyerName = $"{p.Buyer.FirstName} {p.Buyer.LastName}"
                })
                .OrderBy(p => p.Price)
                .Take(10)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            XmlRootAttribute root = new XmlRootAttribute("Products");
            XmlSerializer serializer = new XmlSerializer(typeof(ProductOutputDto[]), root);
            using StringWriter writer = new StringWriter(sb);
            
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            serializer.Serialize(writer, dtos, namespaces);

            return sb.ToString().TrimEnd();
        }

        //Problem 4
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            XmlRootAttribute root = new XmlRootAttribute("CategoryProducts");
            XmlSerializer serializer = new XmlSerializer(typeof(CategoryProductInputDto[]), root);
            using StringReader reader = new StringReader(inputXml);
            CategoryProductInputDto[] dtos = (CategoryProductInputDto[])serializer.Deserialize(reader);

            ICollection<CategoryProduct> categoryProducts = new List<CategoryProduct>();

            foreach(CategoryProductInputDto dto in dtos)
            {
                if (dto.ProductId == 0 || dto.CategoryId == 0)
                {
                    continue;
                }

                CategoryProduct categoryProduct = new CategoryProduct()
                {
                    ProductId = dto.ProductId,
                    CategoryId = dto.CategoryId
                };

                categoryProducts.Add(categoryProduct);
            }

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count}";
        }

        //Problem 3
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            XmlRootAttribute root = new XmlRootAttribute("Categories");
            XmlSerializer serializer = new XmlSerializer(typeof(CategoryInputDto[]), root);
            using StringReader stringReader = new StringReader(inputXml);
            CategoryInputDto[] dtos = (CategoryInputDto[])serializer.Deserialize(stringReader);

            ICollection<Category> categories = new List<Category>();

            foreach (CategoryInputDto dto in dtos)
            {
                if (dto.Name == null)
                {
                    continue;
                }

                Category category = new Category() { Name = dto.Name};
                categories.Add(category);
            }

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }

        //problem 2
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            XmlRootAttribute root = new XmlRootAttribute("Products");
            XmlSerializer serializer = new XmlSerializer(typeof(ProductInputDto[]), root);

            using StringReader reader = new StringReader(inputXml);
            ProductInputDto[] dtos = (ProductInputDto[])serializer.Deserialize(reader);

            MapperConfiguration configuration = new MapperConfiguration(cfg => cfg.AddProfile<ProductShopProfile>());
            IMapper mapper = new Mapper(configuration);

            ICollection<Product> products = mapper.Map<ICollection<Product>>(dtos);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }

        //Problem 1
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute("Users");
            XmlSerializer serializer = new XmlSerializer(typeof(UserInputDto[]), xmlRoot);

            using StringReader stringReader = new StringReader(inputXml);
            UserInputDto[] dtos = (UserInputDto[])serializer.Deserialize(stringReader);

            ICollection<User> users = new List<User>();

            foreach (UserInputDto dto in dtos)
            {
                User user = new User()
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Age = dto.Age
                };
                users.Add(user);
            }

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }
    }
}