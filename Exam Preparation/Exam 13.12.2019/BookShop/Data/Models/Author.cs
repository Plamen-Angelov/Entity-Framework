using BookShop.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookShop.Data.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(GlobalConstants.Author_FirstName_MaxLenght)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(GlobalConstants.Author_LastName_MaxLenght)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(GlobalConstants.Author_Phone_MaxLenght)]
        //[RegularExpression(GlobalConstants.Author_PhoneNumber_Regex)]
        public string Phone { get; set; }

        public ICollection<AuthorBook> AuthorsBooks { get; set; }

        public Author()
        {
            AuthorsBooks = new HashSet<AuthorBook>();
        }
    }
}
