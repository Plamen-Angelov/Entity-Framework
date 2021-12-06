using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Theatre.Common;

namespace Theatre.DataProcessor.ImportDto
{
    public class TheatreImportDto
    {
        [Required]
        [StringLength(GlobalConstants.Thetre_Name_MaxLenght, MinimumLength = GlobalConstants.Thetre_Name_MinLenght)]
        public string Name { get; set; }

        [Required]
        [Range(GlobalConstants.Thetre_NumberOfHalls_MinValue, GlobalConstants.Thetre_NumberOfHalls_MaxValue)]
        public sbyte NumberOfHalls { get; set; }

        [Required]
        [StringLength(GlobalConstants.Thetre_Director_MaxLenght, MinimumLength = GlobalConstants.Thetre_Director_MinLenght)]
        public string Director { get; set; }

        public List<TicketImportDto> Tickets { get; set; }
    }
}
