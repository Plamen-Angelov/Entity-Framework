namespace CarDealer.DTO.Output
{
    public class SaleOutputDto
    {
        public CarOutputDto Car { get; set; }

        public string CustomerName { get; set; }

        public string Discount { get; set; }

        public string Price { get; set; }

        public string PriceWithDiscount { get; set; }
    }
}
