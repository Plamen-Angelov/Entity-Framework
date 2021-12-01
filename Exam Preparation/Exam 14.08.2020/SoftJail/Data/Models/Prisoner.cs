using SoftJail.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftJail.Data.Models
{
    public class Prisoner
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(GlobalConstants.Prisoner_FullName_MaxLenght)]
        public string FullName { get; set; }

        [Required]
        [RegularExpression(GlobalConstants.Prisoner_NickName_Regex)]
        public string Nickname { get; set; }

        [Required]
        [Range(GlobalConstants.Prisoner_Age_MinValue, GlobalConstants.Prisoner_Age_MaxValue)]
        public int Age { get; set; }

        [Required]
        public DateTime IncarcerationDate { get; set; }

        public DateTime? ReleaseDate { get; set; }

        [Range(GlobalConstants.Prisoner_Bail_MinValue, (double)decimal.MaxValue)]
        public decimal? Bail { get; set; }

        [ForeignKey(nameof(Cell))]
        public int? CellId { get; set; }

        public Cell Cell { get; set; }

        public ICollection<Mail> Mails { get; set; }

        public ICollection<OfficerPrisoner> PrisonerOfficers { get; set; }

        public Prisoner()
        {
            Mails = new HashSet<Mail>();
            PrisonerOfficers = new HashSet<OfficerPrisoner>();
        }
    }
}