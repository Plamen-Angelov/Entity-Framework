using Artillery.Common;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
    [XmlType("Shell")]
    public class ImportShellDto
    {
        [Required]
        [XmlElement]
        [Range(GlobalConstants.Shell_Weight_Min, GlobalConstants.Shell_Weight_Max)]
        public double ShellWeight { get; set; }

        [Required]
        [XmlElement]
        [StringLength(GlobalConstants.Shell_Caliber_Max, MinimumLength = GlobalConstants.Shell_Caliber_Min)]
        public string Caliber { get; set; }
    }
}
