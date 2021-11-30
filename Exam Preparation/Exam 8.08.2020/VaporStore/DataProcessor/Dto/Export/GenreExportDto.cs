using System.Collections.Generic;

namespace VaporStore.DataProcessor.Dto.Export
{
    public class GenreExportDto
    {
        public int Id { get; set; }

        public string Genre { get; set; }

        public ICollection<GameExportDto> Games { get; set; }

        public int TotalPlayers { get; set; }
    }
}
