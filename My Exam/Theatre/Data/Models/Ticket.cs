using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Theatre.Common;

namespace Theatre.Data.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(GlobalConstants.Ticket_Price_MinValue, GlobalConstants.Ticket_Price_MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [Range(GlobalConstants.Ticket_RowNumber_MinValue, GlobalConstants.Ticket_RowNumber_MaxValue)]
        public sbyte RowNumber { get; set; }

        [Required]
        [ForeignKey(nameof(Play))]
        public int PlayId { get; set; }

        public Play Play { get; set; }

        [Required]
        [ForeignKey(nameof(Theatre))]
        public int TheatreId { get; set; }

        public Theatre Theatre { get; set; }
    }
}
