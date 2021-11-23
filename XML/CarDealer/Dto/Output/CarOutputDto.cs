﻿using System.Xml.Serialization;

namespace CarDealer.Dto.Output
{
    [XmlType("car")]
    public class CarOutputDto
    {
        [XmlAttribute("make")]
        public string Make { get; set; }

        [XmlAttribute("model")]
        public string Model { get; set; }

        [XmlAttribute("travelled-distance")]
        public long TravelledDistance { get; set; }
    }
}