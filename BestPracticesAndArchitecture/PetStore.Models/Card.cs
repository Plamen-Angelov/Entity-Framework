using PetShop.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetStore.Models
{
    public class Card
    {
        public Card()
        {
            ProductSales = new HashSet<ProductSale>();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(CardValidationConstants.HOLDER_MAX_LENGHT)]
        public string HolderName { get; set; }

        [Required]
        [StringLength(CardValidationConstants.NUMBER_MAX_LENGHT)]
        public string Number { get; set; }

        [Required]
        [StringLength(CardValidationConstants.EXPIRE_DATE_MAX_LENGHT)]
        public string ExpireDate { get; set; }
        [Required]
        [StringLength(CardValidationConstants.CVC_MAX_LEGHT)]
        public string CVC { get; set; }

        [Required]
        [ForeignKey(nameof(Owner))]
        public Guid ClientId { get; set; }
        public virtual Client Owner { get; set; }

        public virtual ICollection<ProductSale> ProductSales { get; set; }
    }
}
