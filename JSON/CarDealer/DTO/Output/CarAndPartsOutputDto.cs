using System.Collections.Generic;

namespace CarDealer.DTO.Output
{
    public class CarAndPartsOutputDto
    {
        public CarOutputDto Car { get; set; }

        public List<PartOutputDto> Parts { get; set; }
    }
}
