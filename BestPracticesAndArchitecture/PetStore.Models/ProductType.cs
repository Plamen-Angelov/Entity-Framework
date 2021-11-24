using PetShop.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PetStore.Models
{
    public class ProductType
    {
        public ProductType()
        {
            Products = new HashSet<Product>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(ProductTypeValidationConstants.NAME_MAX_LENGHT)]
        public string TypeName { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
