using System.Xml.Serialization;

namespace Artillery.DataProcessor.ExportDto
{
    [XmlType("Gun")]
    public class ExportGunCountriesDto
    {
        [XmlAttribute]
        public string Manufacturer { get; set; }

        [XmlAttribute]
        public string GunType { get; set; }

        [XmlAttribute]
        public string GunWeight { get; set; }

        [XmlAttribute("BarrelLength")]
        public string BarrelLength { get; set; }

        [XmlAttribute]
        public string Range { get; set; }

        public ExportCountryDto[] Countries { get; set; }
    }
}
