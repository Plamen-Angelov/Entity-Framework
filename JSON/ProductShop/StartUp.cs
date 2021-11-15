using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            ProductShopContext context = new ProductShopContext();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            string usersJson = File.ReadAllText("../../../datasets/users.json");
            
            Console.WriteLine(ImportUsers(context, usersJson));
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            ICollection<User> users = JsonConvert.DeserializeObject<ICollection<User>>(inputJson);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });

            IMapper mapper = new Mapper(config);
            ICollection<User> mappedUsers = mapper.Map<ICollection<User>>(users);

            context.Users.AddRange(mappedUsers);
            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }
    }
}