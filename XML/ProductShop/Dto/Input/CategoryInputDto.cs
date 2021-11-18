using System.Xml.Serialization;

namespace ProductShop.Dto.Input
{
    [XmlType("Category")]
    public class CategoryInputDto
    {
        [XmlElement("name")]
        public string Name { get; set; }
    }
}
