using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Theatre.Common;

namespace Theatre.Data.Models
{
    public class Cast
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(GlobalConstants.Cast_FullName_MaxLenght)]
        public string FullName { get; set; }

        [Required]
        public bool IsMainCharacter { get; set; }

        [Required]
        [RegularExpression(GlobalConstants.Cast_PhoneNumber_Regex)]
        public string PhoneNumber { get; set; }

        [Required]
        [ForeignKey(nameof(Play))]
        public int PlayId { get; set; }

        public Play Play { get; set; }
    }
}
