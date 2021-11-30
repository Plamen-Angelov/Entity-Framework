using System.ComponentModel.DataAnnotations;
using VaporStore.Common;

namespace VaporStore.DataProcessor.Dto.Import
{
    public class CardImportDto
    {
        [Required]
        [RegularExpression(GlobalConstants.Card_Number_Regex)]
        public string Number { get; set; }

        [Required]
        [RegularExpression(GlobalConstants.Card_Cvc_Regex)]
        public string Cvc { get; set; }

        [Required]
        public string Type { get; set; }
    }
}
