using PetShop.Common;
using PetStore.Models.Enumerations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetStore.Models
{
    public class Pet
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey(nameof(PetType))]
        public int PetTypeId { get; set; }
        public virtual PetType PetType { get; set; }

        [Required]
        [StringLength(PetValidationConstants.NAME_MAX_LEGTH)]
        public string Name { get; set; }

        [Required]
        [ForeignKey(nameof(Breed))]
        public int BreedId { get; set; }
        public virtual Breed Breed { get; set; }

        public int Age { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [StringLength(PetValidationConstants.DESCRIPTION_MAX_LENGHT)]
        public string Description { get; set; }

        public decimal Price { get; set; }

        public bool IsSold { get; set; }

        [Required]
        [ForeignKey(nameof(Store))]
        public Guid StoreId { get; set; }
        public virtual Store Store { get; set; }

        [ForeignKey(nameof(PetReservation))]
        public Guid? PetReservationId { get; set; }
        public virtual PetReservation PetReservation { get; set; }
    }
}
