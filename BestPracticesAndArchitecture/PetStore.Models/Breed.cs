using PetShop.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PetStore.Models
{
    public class Breed
    {
        public Breed()
        {
               Pets = new HashSet<Pet>(); 
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(BreedValidationConstants.BREEDNAME_MAX_LENGHT)]
        public string BreedName { get; set; }

        public virtual ICollection<Pet> Pets { get; set; }
    }
}
