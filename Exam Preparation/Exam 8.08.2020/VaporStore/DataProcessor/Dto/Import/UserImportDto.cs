using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VaporStore.Common;

namespace VaporStore.DataProcessor.Dto.Import
{
    public class UserImportDto
    {

        [Required]
        [MinLength(GlobalConstants.User_UserName_MinLenght)]
        [MaxLength(GlobalConstants.User_UserName_MaxLenght)]
        public string Username { get; set; }

        [Required]
        [RegularExpression(GlobalConstants.User_FullName_Regex)]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [Range(GlobalConstants.User_Age_Min, GlobalConstants.User_Age_Max)]
        public int Age { get; set; }

        [MinLength(1)]
        public List<CardImportDto> Cards { get; set; }
    }
}
