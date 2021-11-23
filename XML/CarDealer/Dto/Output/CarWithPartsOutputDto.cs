using System.Collections.Generic;
using System.Xml.Serialization;

namespace CarDealer.Dto.Output
{
    [XmlType("car")]
    public class CarWithPartsOutputDto
    {
        [XmlAttribute("make")]
        public string Make { get; set; }

        [XmlAttribute("model")]
        public string Model { get; set; }

        [XmlAttribute("travelled-distance")]
        public long TravelledDistance { get; set; }

        [XmlArray("parts")]
        public List<PartOutputDto> Parts { get; set; }
    }
}
