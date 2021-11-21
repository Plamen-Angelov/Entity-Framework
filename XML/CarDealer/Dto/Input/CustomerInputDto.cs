using System;
using System.Xml.Serialization;

namespace CarDealer.Dto.Input
{
    [XmlType("Customer")]
    public class CustomerInputDto
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("birthDate")]
        public DateTime BirthDate { get; set; }

        [XmlElement("isYoungDriver")]
        public bool IsYoungDriver { get; set; }
    }
}
