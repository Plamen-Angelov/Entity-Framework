using BookShop.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookShop.DataProcessor.ImportDto
{
    public class AuthorImportDto
    {
        [Required]
        [StringLength(GlobalConstants.Author_FirstName_MaxLenght, MinimumLength = GlobalConstants.Author_FirstName_MinLenght)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(GlobalConstants.Author_LastName_MaxLenght, MinimumLength = GlobalConstants.Author_LastName_MinLenght)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(GlobalConstants.Author_Phone_MaxLenght)]
        [RegularExpression(GlobalConstants.Author_PhoneNumber_Regex)]
        public string Phone { get; set; }

        public List<AuthorsBooksImportDto> Books { get; set; }
    }
}
