using PetShop.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetStore.Models
{
    public class Address
    {
        public Address()
        {
            Clients = new HashSet<Client>();
            ProductSales = new HashSet<ProductSale>();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(AddressValidationConstants.TOWN_MAX_LENGHT)]
        public string Town { get; set; }

        [Required]
        [StringLength(AddressValidationConstants.TEXT_MAX_LENGHT)]
        public string Text { get; set; }

        [StringLength(AddressValidationConstants.NOTES_MAX_LENGHT)]
        public string Notes { get; set; }

        [ForeignKey(nameof(Store))]
        public Guid StoreId { get; set; }
        public virtual Store Store { get; set; }

        public virtual ICollection<Client> Clients { get; set; }

        public virtual ICollection<ProductSale> ProductSales { get; set; }
    }
}
