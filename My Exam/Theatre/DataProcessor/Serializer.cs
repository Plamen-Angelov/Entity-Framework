namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            TheatreExportDto[] thetres = context
                .Theatres
                .ToArray()
                .Where(t => t.NumberOfHalls >= numbersOfHalls && t.Tickets.Count() >= 20)
                .Select(t => new TheatreExportDto()
                {
                    Name = t.Name,
                    Halls = t.NumberOfHalls,
                    TotalIncome = t.Tickets
                    .ToArray()
                    .Where(ti => ti.RowNumber <= 5)
                    .Select(ti => ti.Price)
                    .Sum(),
                    Tickets = t.Tickets
                    .ToArray()
                    .Where(ti => ti.RowNumber <= 5)
                    .Select(ti => new TicketExportDto()
                    {
                        Price = ti.Price,
                        RowNumber = ti.RowNumber
                    })
                    .OrderByDescending(ti => ti.Price)
                    .ToList()
                })
                .OrderByDescending(t => t.Halls)
                .ThenBy(t => t.Name)
                .ToArray();

            string json = JsonConvert.SerializeObject(thetres, Formatting.Indented);
            return json;
        }

        public static string ExportPlays(TheatreContext context, double rating)
        {
            PlayExportDto[] plays = context
                .Plays
                .ToArray()
                .Where(p => p.Rating <= rating)
                .Select(p => new PlayExportDto()
                {
                    Title = p.Title,
                    Duration = p.Duration.ToString("c"),
                    Rating = p.Rating == 0? "Premier" : p.Rating.ToString(),
                    //Rating = p.Rating.ToString(),
                    Genre = p.Genre.ToString(),
                    Actors = p.Casts
                    .ToArray()
                    .Where(a => a.IsMainCharacter == true)
                    .Select(a => new ActorExportDto()
                    {
                        FullName = a.FullName,
                        MainCharacter = $"Plays main character in '{a.Play.Title}'."
                    })
                    .OrderByDescending(a => a.FullName)
                    .ToList()
                })
                .OrderBy(p => p.Title)
                .ThenByDescending(p => p.Genre)
                .ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(PlayExportDto[]), new XmlRootAttribute("Plays"));
            StringBuilder sb = new StringBuilder();

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);

            using StringWriter writer = new StringWriter(sb);
            serializer.Serialize(writer, plays, ns);

            return sb.ToString().TrimEnd();
        }
    }
}
