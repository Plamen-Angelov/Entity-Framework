using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VaporStore.Common;
using VaporStore.Data.Models.Enums;

namespace VaporStore.Data.Models
{
    public class Card
    {
        public Card()
        {
            Purchases = new HashSet<Purchase>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(GlobalConstants.Card_Number_MaxLenght)]
        public string Number { get; set; }

        [Required]
        [StringLength(GlobalConstants.Card_Cvc_MaxLenght)]
        public string Cvc { get; set; }

        [Required]
        public CardType Type { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<Purchase> Purchases { get; set; }
    }
}
