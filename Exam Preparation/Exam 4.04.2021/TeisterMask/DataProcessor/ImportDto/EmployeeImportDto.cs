using System.ComponentModel.DataAnnotations;
using TeisterMask.Data.Models;

namespace TeisterMask.DataProcessor.ImportDto
{
    public class EmployeeImportDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(Constants.EMPLOYEE_NAME_MAX_LENGHT)]
        [RegularExpression(@"[a-zA-Z0-9]+")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"[\d]{3}-[\d]{3}-[\d]{4}")]
        public string Phone { get; set; }

        public int[] Tasks { get; set; }
    }
}
