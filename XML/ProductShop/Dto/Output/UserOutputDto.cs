using System.Collections.Generic;
using System.Xml.Serialization;

namespace ProductShop.Dto.Output
{
    [XmlType("User")]
    public class UserOutputDto
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }

        [XmlElement("lastName")]
        public string LastName { get; set; }

        [XmlElement("soldProducts")]
        public List<ProductOutputDto> soldProducts { get; set; }
    }
}
