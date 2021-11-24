using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetStore.Models
{
    public class PetReservation
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey(nameof(Client))]
        public Guid ClientId { get; set; }
        public virtual Client Client { get; set; }

        [Required]
        [ForeignKey(nameof(Pet))]
        public Guid PetId { get; set; }
        public virtual Pet Pet { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        [Required]
        [ForeignKey(nameof(Store))]
        public Guid StoreId { get; set; }
        public virtual Store Store { get; set; }

        [Required]
        public DateTime ReservedOn { get; set; }
    }
}
