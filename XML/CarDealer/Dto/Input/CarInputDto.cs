using System.Collections.Generic;
using System.Xml.Serialization;

namespace CarDealer.Dto.Input
{
    [XmlType("Car")]
    public class CarInputDto
    {
        [XmlElement("make")]
        public string Make { get; set; }

        [XmlElement("model")]
        public string Model { get; set; }

        [XmlElement("TraveledDistance")]
        public long TravelledDistance { get; set; }

        [XmlArray("parts")]
        public PartIdInputDto[] PartIds { get; set; }
    }
}
