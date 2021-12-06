using System.ComponentModel.DataAnnotations;
using Theatre.Common;

namespace Theatre.DataProcessor.ImportDto
{
    public class TicketImportDto
    {
        [Required]
        [Range(GlobalConstants.Ticket_Price_MinValue, GlobalConstants.Ticket_Price_MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [Range(GlobalConstants.Ticket_RowNumber_MinValue, GlobalConstants.Ticket_RowNumber_MaxValue)]
        public sbyte RowNumber { get; set; }

        [Required]
        public int PlayId { get; set; }
    }
}
