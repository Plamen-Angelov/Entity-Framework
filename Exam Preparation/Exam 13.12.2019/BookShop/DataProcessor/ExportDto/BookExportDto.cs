using System;
using System.Xml.Serialization;

namespace BookShop.DataProcessor.ExportDto
{
    [XmlType("Book")]
    public class BookExportDto
    {
        [XmlAttribute("Pages")]
        public string Pages { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement]
        public string Date { get; set; }
    }
}
