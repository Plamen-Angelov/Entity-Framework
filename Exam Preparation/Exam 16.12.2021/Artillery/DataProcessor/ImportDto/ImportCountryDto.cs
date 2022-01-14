using Artillery.Common;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
    [XmlType("Country")]
    public class ImportCountryDto
    {
        [Required]
        [XmlElement]
        [StringLength(GlobalConstants.Country_CountryName_MaxLenght, MinimumLength =GlobalConstants.Country_CountryName_MinLenght)]
        public string CountryName { get; set; }

        [Required]
        [XmlElement]
        [Range(GlobalConstants.Country_ArmySize_Min, GlobalConstants.Country_ArmySize_Max)]
        public int ArmySize { get; set; }
    }
}
