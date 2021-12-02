using SoftJail.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ImportDto
{
    [XmlType("Officer")]
    public class OfficerImportDto
    {
        [Required]
        [MinLength(GlobalConstants.Officer_FullName_MinLenght)]
        [MaxLength(GlobalConstants.Officer_FullName_MaxLenght)]
        [XmlElement("Name")]
        public string FullName { get; set; }

        [Required]
        [Range(GlobalConstants.Officer_Salary_MinValue, (double)decimal.MaxValue)]
        [XmlElement("Money")]
        public decimal Salary { get; set; }

        [Required]
        [XmlElement("Position")]
        public string Position { get; set; }

        [Required]
        [XmlElement("Weapon")]
        public string Weapon { get; set; }

        [Required]
        [XmlElement("DepartmentId")]
        public int DepartmentId { get; set; }

        [XmlArray("Prisoners")]
        public List<PrisonerDto> Prisoners { get; set; }
    }
}
