using PetShop.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetStore.Models
{
    public class Client
    {
        public Client()
        {
            PetReservations = new HashSet<PetReservation>();
            ProductSales = new HashSet<ProductSale>();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(ClientValidationConstants.NAME_MAX_LENGHT)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(ClientValidationConstants.NAME_MAX_LENGHT)]
        public string LastName { get; set; }

        [Required]
        [StringLength(ClientValidationConstants.PASSWORD_MAX_LENGHT)]
        public string Password { get; set; }

        [Required]
        [StringLength(ClientValidationConstants.PHONE_NUMBER_MAX_LENGHT)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(ClientValidationConstants.EMAIL_MAX_LENGHT)]
        public string Email { get; set; }

        [ForeignKey(nameof(Address))]
        public Guid? AddressId { get; set; }
        public virtual Address Address { get; set; }

        [ForeignKey(nameof(Card))]
        public Guid? CardId { get; set; }
        public virtual Card Card { get; set; }

        public virtual ICollection<PetReservation> PetReservations { get; set; }

        public virtual ICollection<ProductSale> ProductSales { get; set; }

    }
}
