using Artillery.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artillery.DataProcessor.ImportDto
{
    public class ImportGunDto
    {
        [Required]
        public int ManufacturerId { get; set; }

        [Required]
        [Range(GlobalConstants.Gun_Weight_Min, GlobalConstants.Gun_Weight_Max)]
        public int GunWeight { get; set; }

        [Required]
        [Range(GlobalConstants.Gun_BarrelLenght_Min, GlobalConstants.Gun_BarrelLenght_Max)]
        public double BarrelLength { get; set; }

        public int? NumberBuild { get; set; }

        [Required]
        [Range(GlobalConstants.Gun_Range_Min, GlobalConstants.Gun_Range_Max)]
        public int Range { get; set; }

        [Required]
        public string GunType { get; set; }

        [Required]
        public int ShellId { get; set; }

        public List<ImportGunCountryDto> Countries { get; set; }
    }
}
