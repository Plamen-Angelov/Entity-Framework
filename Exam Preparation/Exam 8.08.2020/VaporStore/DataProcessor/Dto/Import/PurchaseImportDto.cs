using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using VaporStore.Common;

namespace VaporStore.DataProcessor.Dto.Import
{
    [XmlType("Purchase")]
    public class PurchaseImportDto
    {
        [Required]
        [XmlAttribute("title")]
        public string GameName { get; set; }

        [Required]
        [XmlElement]
        public string Type { get; set; }

        [Required]
        [XmlElement("Key")]
        [RegularExpression(GlobalConstants.Purchase_Key_Regex)]
        public string ProductKey { get; set; }

        [Required]
        [XmlElement("Card")]
        [RegularExpression(GlobalConstants.Card_Number_Regex)]
        public string CardNumber { get; set; }

        [Required]
        [XmlElement]
        public string Date { get; set; }
    }
}
