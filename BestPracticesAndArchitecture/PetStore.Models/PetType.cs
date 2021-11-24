using PetShop.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PetStore.Models
{
    public class PetType
    {
        public PetType()
        {
            Pets = new HashSet<Pet>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(PetTypeValidationConstants.TYPE_NAME_MAX_LENGHT)]
        public string TypeName { get; set; }

        public virtual ICollection<Pet> Pets { get; set; }
    }
}
