using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VaporStore.Common;

namespace VaporStore.DataProcessor.Dto.Import
{
    public class GameImportDto
    {
        public GameImportDto()
        {
            Tags = new List<string>();
        }

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(GlobalConstants.Game_Min_PriceValue, (double)decimal.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public string ReleaseDate { get; set; }

        [Required]
        public string Developer { get; set; }

        [Required]
        public string Genre { get; set; }

        [MinLength(1)]
        public List<string> Tags { get; set; }
    }
}
