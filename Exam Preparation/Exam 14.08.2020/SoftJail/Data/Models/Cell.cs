using SoftJail.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftJail.Data.Models
{
    public class Cell
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(GlobalConstants.Cell_Number_MinLenght, GlobalConstants.Cell_Number_MaxLenght)]
        public int CellNumber { get; set; }

        [Required]
        public bool HasWindow { get; set; }

        [Required]
        [ForeignKey(nameof(Department))]
        public int DepartmentId { get; set; }

        [Required]
        public Department Department { get; set; }

        public ICollection<Prisoner> Prisoners { get; set; }

        public Cell()
        {
            Prisoners = new HashSet<Prisoner>();
        }
    }
}
