
using System.Xml.Serialization;

namespace CarDealer.Dto.Input
{
    [XmlType("partId")]
    public class PartIdInputDto
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}
