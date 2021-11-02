using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P03_FootballBetting.Data.Models
{
    public class PlayerStatistic
    {
        [ForeignKey(nameof(Player))]
        public int PlayerId { get; set; }

        public virtual Player Player { get; set; }

        [ForeignKey(nameof(Game))]
        public int GameId { get; set; }

        public virtual Game Game { get; set; }

        public byte ScoredGoals { get; set; }

        [Range(0, 99)]
        public byte Assists { get; set; }

        [Range(0, 250)]
        public byte MinutesPlayed { get; set; }

    }
}
