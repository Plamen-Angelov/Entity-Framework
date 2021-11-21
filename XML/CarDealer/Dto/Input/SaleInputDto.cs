using System.Xml.Serialization;

namespace CarDealer.Dto.Input
{
    [XmlType("Sale")]
    public class SaleInputDto
    {
        [XmlElement("carId")]
        public int CarId { get; set; }

        [XmlElement("customerId")]
        public int CustomerId { get; set; }

        [XmlElement("discount")]
        public decimal Discount { get; set; }

    }
}
