using Artillery.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Artillery.Data.Models
{
    public class Country
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(GlobalConstants.Country_CountryName_MaxLenght)]
        public string CountryName { get; set; }

        [Required]
        [Range(GlobalConstants.Country_ArmySize_Min, GlobalConstants.Country_ArmySize_Max)]
        public int ArmySize { get; set; }

        public ICollection<CountryGun> CountriesGuns { get; set; }

        public Country()
        {
            CountriesGuns = new HashSet<CountryGun>();
        }
    }
}
