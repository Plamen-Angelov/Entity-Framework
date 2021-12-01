using SoftJail.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto
{
    public class PrisonerImportDto
    {
        [Required]
        [MinLength(GlobalConstants.Prisoner_FullName_MinLenght)]
        [MaxLength(GlobalConstants.Prisoner_FullName_MaxLenght)]
        public string FullName { get; set; }

        [Required]
        [RegularExpression(GlobalConstants.Prisoner_NickName_Regex)]
        public string Nickname { get; set; }

        [Required]
        [Range(GlobalConstants.Prisoner_Age_MinValue, GlobalConstants.Prisoner_Age_MaxValue)]
        public int Age { get; set; }

        [Required]
        public string IncarcerationDate { get; set; }

        public string ReleaseDate { get; set; }

        [Range(GlobalConstants.Prisoner_Bail_MinValue, (double)decimal.MaxValue)]
        public decimal? Bail { get; set; }

        public int CellId { get; set; }

        public List<MailImportDto> Mails { get; set; }
    }
}
