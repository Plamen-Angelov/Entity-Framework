using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductShop.DTOs.Output
{
    public class UsersWithSoldProductsOutputDto
    {
        public int Count { get; set; }

        public ICollection<Object> Users { get; set; }
    }
}
