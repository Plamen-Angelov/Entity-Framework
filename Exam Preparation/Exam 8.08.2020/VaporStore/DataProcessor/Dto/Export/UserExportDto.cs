using System.Collections.Generic;
using System.Xml.Serialization;

namespace VaporStore.DataProcessor.Dto.Export
{
    [XmlType("User")]
    public class UserExportDto
    {
        [XmlAttribute("username")]
        public string UserName { get; set; }

        [XmlArray("Purchases")]
        public List<PurchaseExportDto> Purchases { get; set; }

        [XmlElement("TotalSpent")]
        public decimal TotalSpent { get; set; }
    }
}
