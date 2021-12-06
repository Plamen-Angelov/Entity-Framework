using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Theatre.Common;

namespace Theatre.Data.Models
{
    public class Theatre
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(GlobalConstants.Thetre_Name_MaxLenght)]
        public string Name { get; set; }

        [Required]
        [Range(GlobalConstants.Thetre_NumberOfHalls_MinValue, GlobalConstants.Thetre_NumberOfHalls_MaxValue)]
        public sbyte NumberOfHalls { get; set; }

        [Required]
        [StringLength(GlobalConstants.Thetre_Director_MaxLenght)]
        public string Director { get; set; }

        public ICollection<Ticket> Tickets { get; set; }

        public Theatre()
        {
            Tickets = new HashSet<Ticket>();
        }
    }
}
