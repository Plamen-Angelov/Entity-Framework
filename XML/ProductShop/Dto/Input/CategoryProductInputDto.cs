using System.Xml.Serialization;

namespace ProductShop.Dto.Input
{
    [XmlType("CategoryProduct")]
    public class CategoryProductInputDto
    {
        [XmlElement("CategoryId")]
        public int CategoryId { get; set; }

        [XmlElement("ProductId")]
        public int ProductId { get; set; }
    }
}
