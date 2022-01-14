using Artillery.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Artillery.Data.Models
{
    public class Manufacturer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(GlobalConstants.Manufactorer_Name_Max)]
        public string ManufacturerName { get; set; }

        [Required]
        [StringLength(GlobalConstants.Manufactorer_Founder_Max)]
        public string Founded { get; set; }

        public ICollection<Gun> Guns { get; set; }

        public Manufacturer()
        {
            Guns = new HashSet<Gun>();
        }
    }
}
