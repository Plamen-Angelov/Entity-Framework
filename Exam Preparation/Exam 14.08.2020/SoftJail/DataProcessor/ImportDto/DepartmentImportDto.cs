using SoftJail.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto
{
    public class DepartmentImportDto
    {
        [Required]
        [MinLength(GlobalConstants.Department_Name_MinLenght)]
        [MaxLength(GlobalConstants.Department_Name_MaxLenght)]
        public string Name { get; set; }

        [MinLength(1)]
        public List<CellImportDto> Cells { get; set; }

        public DepartmentImportDto()
        {
            Cells = new List<CellImportDto>();
        }
    }
}
