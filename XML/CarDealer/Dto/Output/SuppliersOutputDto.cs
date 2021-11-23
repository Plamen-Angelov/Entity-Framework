using System.Xml.Serialization;

namespace CarDealer.Dto.Output
{
    [XmlType("suplier")]
    public class SuppliersOutputDto
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("parts-count")]
        public int Count { get; set; }
    }
}
