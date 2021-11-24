using PetShop.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetStore.Models
{
    public class Product
    {
        public Product()
        {
            ProductSales = new HashSet<ProductSale>();
            AvailableStores = new HashSet<Store>();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey(nameof(Type))]
        public int ProductTypeId { get; set; }
        public virtual ProductType Type { get; set; }

        [Required]
        [StringLength(ProductValidationConstants.NAME_MAX_LENGHT)]
        public string Name { get; set; }

        [StringLength(ProductValidationConstants.DESCRIPTION_MAX_LENGHT)]
        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public bool InStock
            => this.Quantity > 0;

        [StringLength(ProductValidationConstants.DISTRIBUTOR_NAME_MAX_LENGHT)]
        public string Distributor { get; set; }

        public virtual ICollection<Store> AvailableStores { get; set; }

        public virtual ICollection<ProductSale> ProductSales { get; set; }
    }
}
