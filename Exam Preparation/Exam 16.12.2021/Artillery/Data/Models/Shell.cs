using Artillery.Common;
using Artillery.Data.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Artillery.Data
{
    public class Shell
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public double ShellWeight { get; set; }

        [Required]
        [StringLength(GlobalConstants.Shell_Caliber_Max)]
        public string Caliber { get; set; }

        public ICollection<Gun> Guns { get; set; }

        public Shell()
        {
            Guns = new HashSet<Gun>();
        }
    }
}
