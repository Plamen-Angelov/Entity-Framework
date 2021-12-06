namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;
    using Theatre.Common;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";

        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PlayImportDto[]), new XmlRootAttribute("Plays"));

            using StringReader reader = new StringReader(xmlString);

            PlayImportDto[] dtos = (PlayImportDto[])serializer.Deserialize(reader);
            List<Play> plays = new List<Play>();
            StringBuilder sb = new StringBuilder();

            foreach (var dto in dtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(GlobalConstants.Error_Message);
                    continue;
                }

                bool isDurationValid = TimeSpan.TryParseExact(dto.Duration, "c", 
                    CultureInfo.InvariantCulture, out TimeSpan duration);

                if (!isDurationValid)
                {
                    sb.AppendLine(GlobalConstants.Error_Message);
                    continue;
                }

                if (duration < new TimeSpan(1, 0, 0))
                {
                    sb.AppendLine(GlobalConstants.Error_Message);
                    continue;
                }

                bool isGenreValid = Enum.TryParse(typeof(Genre), dto.Genre, out object genre);

                if (!isGenreValid)
                {
                    sb.AppendLine(GlobalConstants.Error_Message);
                    continue;
                }

                Play play = new Play()
                {
                    Title = dto.Title,
                    Duration = duration,
                    Rating = dto.Rating,
                    Genre = (Genre)genre,
                    Description = dto.Description,
                    Screenwriter = dto.Screenwriter
                };

                plays.Add(play);
                sb.AppendLine($"Successfully imported {play.Title} with genre {play.Genre.ToString()} " +
                    $"and a rating of {play.Rating}!");
            }

            context.AddRange(plays);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CastImportDto[]), new XmlRootAttribute("Casts"));

            using StringReader reader = new StringReader(xmlString);

            CastImportDto[] dtos = (CastImportDto[])serializer.Deserialize(reader);
            List<Cast> casts = new List<Cast>();
            StringBuilder sb = new StringBuilder();

            foreach (var dto in dtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(GlobalConstants.Error_Message);
                    continue;
                }

                Cast cast = new Cast()
                {
                    FullName = dto.FullName,
                    IsMainCharacter = dto.IsMainCharacter,
                    PhoneNumber = dto.PhoneNumber,
                    PlayId = dto.PlayId
                };

                string characterType = string.Empty;
                
                if (cast.IsMainCharacter)
                {
                    characterType = "main";
                }
                else
                {
                    characterType = "lesser";
                }

                casts.Add(cast);
                sb.AppendLine($"Successfully imported actor {cast.FullName} as a {characterType} character!");
            }

            context.AddRange(casts);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            TheatreImportDto[] dtos = JsonConvert.DeserializeObject<TheatreImportDto[]>(jsonString);
            List<Theatre> theatres = new List<Theatre>();
            StringBuilder sb = new StringBuilder();

            foreach (var dto in dtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(GlobalConstants.Error_Message);
                    continue;
                }

                Theatre theatre = new Theatre()
                {
                    Name = dto.Name,
                    NumberOfHalls = dto.NumberOfHalls,
                    Director = dto.Director
                };

                List<Ticket> tickets = new List<Ticket>();

                foreach (var t in dto.Tickets)
                {
                    if (!IsValid(t))
                    {
                        sb.AppendLine(GlobalConstants.Error_Message);
                        continue;
                    }

                    Ticket ticket = new Ticket()
                    {
                        Price = t.Price,
                        RowNumber = t.RowNumber,
                        PlayId = t.PlayId
                    };

                    tickets.Add(ticket);
                }

                theatre.Tickets = tickets;
                theatres.Add(theatre);
                sb.AppendLine($"Successfully imported theatre {theatre.Name} with #{theatre.Tickets.Count} tickets!");
            }

            context.Theatres.AddRange(theatres);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }


        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
