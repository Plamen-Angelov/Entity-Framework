using System.Collections.Generic;
using System.Xml.Serialization;

namespace ProductShop.Dto.Output
{
    [XmlType("user")]
    public class UserWithProductsCountOutputDto
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }

        [XmlElement("lastName")]
        public string LastName { get; set; }

        [XmlElement("age")]
        public int? Age { get; set; }

        [XmlElement("SoldProducts")]
        public ProductCountDto SoldProducts { get; set; }

        
    }
}
