using SoftJail.Common;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto
{
    public class CellImportDto
    {
        [Required]
        [Range(GlobalConstants.Cell_Number_MinLenght, GlobalConstants.Cell_Number_MaxLenght)]
        public int CellNumber { get; set; }

        [Required]
        public bool HasWindow { get; set; }
    }
}
