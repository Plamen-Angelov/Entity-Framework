using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Theatre.Common;
using Theatre.Data.Models.Enums;

namespace Theatre.Data.Models
{
    public class Play
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(GlobalConstants.Play_Title_MaxLenght)]
        public string Title { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }

        [Required]
        [Range(GlobalConstants.Play_Rating_MinValue, GlobalConstants.Play_Rating_MaxValue)]
        public float Rating { get; set; }

        [Required]
        public Genre Genre { get; set; }

        [Required]
        [StringLength(GlobalConstants.Play_Description_MaxLemght)]
        public string Description { get; set; }

        [Required]
        [StringLength(GlobalConstants.Play_ScreenWriter_MaxLenght)]
        public string Screenwriter { get; set; }

        public ICollection<Cast> Casts { get; set; }

        public ICollection<Ticket> Tickets { get; set; }

        public Play()
        {
            Casts = new HashSet<Cast>();
            Tickets = new HashSet<Ticket>();
        }
    }
}
