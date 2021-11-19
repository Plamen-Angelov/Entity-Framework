using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Dto.Output
{
    [XmlType("SoldProducts")]
    public class ProductCountDto
    {
        [XmlElement("count")]
        public int CountOfSoldProducts { get; set; }

        [XmlElement("products")]
        public List<ProductDto> Products { get; set; }

    }
}
