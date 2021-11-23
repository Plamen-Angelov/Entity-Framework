using System.Xml.Serialization;

namespace CarDealer.Dto.Output
{
    [XmlType("customer")]
    public class CustomerOutputDto
    {
        [XmlAttribute("full-name")]
        public string Name { get; set; }

        [XmlAttribute("bought-cars")]
        public int BoughtCars { get; set; }

        [XmlAttribute("spent-money")]
        public decimal SpentMoney { get; set; }
    }
}
