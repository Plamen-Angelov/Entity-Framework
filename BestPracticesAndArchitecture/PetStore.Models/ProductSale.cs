using PetShop.Common;
using PetStore.Models.Enumerations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetStore.Models
{
    public class ProductSale
    {
        [Required]
        [ForeignKey(nameof(Client))]
        public Guid ClientId { get; set; }
        public virtual Client Client { get; set; }

        [Required]
        [ForeignKey(nameof(Product))]
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; }

        public decimal? Discount { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public PaymentType PaymentType { get; set; }

        [ForeignKey(nameof(Card))]
        public Guid? CardID { get; set; }
        public virtual Card Card { get; set; }

        [ForeignKey(nameof(DeliveryAddress))]
        public Guid AddressId { get; set; }
        public virtual Address DeliveryAddress { get; set; }

        [StringLength(ProductSaleValidationConstants.BILL_INFO_MAX_LENGHT)]
        public string BillInfo { get; set; }
    }
}
