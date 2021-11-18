using System.Xml.Serialization;

namespace ProductShop.Dto.Input
{
    [XmlType("User")]
    public class UserInputDto
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }

        [XmlElement("lastName")]
        public string LastName { get; set; }

        [XmlElement("age")]
        public int? Age { get; set; }
    }
}
