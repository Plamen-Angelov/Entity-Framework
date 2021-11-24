using PetShop.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetStore.Models
{
    public class Store
    {
        public Store()
        {
            PetReservations = new HashSet<PetReservation>();
            Services = new HashSet<Service>();
            Pets = new HashSet<Pet>();
            Products = new HashSet<Product>();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(StoreValidationConstants.NAME_MAX_LENGHT)]
        public string Name { get; set; }

        [Required]
        [StringLength(StoreValidationConstants.EMAIL_MAX_LENGHT)]
        public string Email { get; set; }

        [Required]
        [ForeignKey(nameof(Address))]
        public Guid AddressId { get; set; }
        public virtual Address Address { get; set; }

        [Required]
        [StringLength(StoreValidationConstants.PHONE_NUMBER_MAX_LENGHT)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(StoreValidationConstants.WORKING_TIME_MAX_LENGHT)]
        public string WorkingTime { get; set; }

        public virtual ICollection<PetReservation> PetReservations { get; set; }

        public virtual ICollection<Service> Services { get; set; }

        public virtual ICollection<Pet> Pets { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
