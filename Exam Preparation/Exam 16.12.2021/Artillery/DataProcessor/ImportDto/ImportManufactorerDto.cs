using Artillery.Common;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
    [XmlType("Manufacturer")]
    public class ImportManufactorerDto
    {
        [Required]
        [StringLength(GlobalConstants.Manufactorer_Name_Max, MinimumLength = GlobalConstants.Manufactorer_Name_Min)]
        public string ManufacturerName { get; set; }

        [Required]
        [StringLength(GlobalConstants.Manufactorer_Founder_Max, MinimumLength = GlobalConstants.Manufactorer_Founder_Min)]
        public string Founded { get; set; }
    }
}
