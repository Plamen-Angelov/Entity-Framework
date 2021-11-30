using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VaporStore.Common;

namespace VaporStore.Data.Models
{
    public class User
    {
        public User()
        {
            Cards = new List<Card>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(GlobalConstants.User_UserName_MaxLenght)]
        public string Username { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public int Age { get; set; }

        public ICollection<Card> Cards { get; set; }
    }
}
