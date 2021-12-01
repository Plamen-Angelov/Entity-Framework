using SoftJail.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.Data.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(GlobalConstants.Department_Name_MaxLenght)]
        public string Name { get; set; }

        public ICollection<Cell> Cells { get; set; }

        public Department()
        {
            Cells = new HashSet<Cell>();
        }
    }
}
