using PetShop.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PetStore.Models
{
    public class Service
    {
        public Service()
        {
            Stores = new HashSet<Store>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(ServiceValidationConstants.NAME_MAX_LENGHT)]
        public string Name { get; set; }

        [StringLength(ServiceValidationConstants.DESCRIPTION_MAX_LENGHT)]
        public string Description { get; set; }

        public decimal? Discount { get; set; }

        public decimal TotalPrice { get; set; }

        public virtual ICollection<Store> Stores { get; set; }
    }
}
