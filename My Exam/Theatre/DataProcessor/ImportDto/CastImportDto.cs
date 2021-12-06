using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Theatre.Common;

namespace Theatre.DataProcessor.ImportDto
{
    [XmlType("Cast")]
    public class CastImportDto
    {
        [Required]
        [XmlElement("FullName")]
        [StringLength(GlobalConstants.Cast_FullName_MaxLenght, MinimumLength = GlobalConstants.Cast_FullName_MinLenght)]
        public string FullName { get; set; }

        [Required]
        [XmlElement("IsMainCharacter")]
        public bool IsMainCharacter { get; set; }

        [Required]
        [XmlElement("PhoneNumber")]
        [RegularExpression(GlobalConstants.Cast_PhoneNumber_Regex)]
        public string PhoneNumber { get; set; }

        [Required]
        [XmlElement("PlayId")]
        public int PlayId { get; set; }
    }
}
