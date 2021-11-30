namespace VaporStore.DataProcessor
{
	using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.Dto.Export;

    public static class Serializer
	{
		public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
		{
			GenreExportDto[] genres = context
				.Genres
				.Include(g => g.Games)
				.ThenInclude(g => g.Purchases)
				.Where(g => genreNames.Contains(g.Name))
				.Where(g => g.Games.Count(g => g.Purchases.Any()) > 0)
				//.Where(g => g.Games.Any(g => g.Purchases.Any()))
				.ToArray()
				.Select(g => new GenreExportDto()
				{
					Id = g.Id,
					Genre = g.Name,
					Games = g.Games
					.ToArray()
					.Select(g => new GameExportDto
					{
						Id = g.Id,
						Title = g.Name,
						Developer = g.Developer.Name,
						Tags = string.Join(", ", g.GameTags.Select(gt => gt.Tag.Name)),
						Players = g.Purchases.Count()
					})
					.Where(g => g.Players > 0)
					.OrderByDescending(g => g.Players)
					.ThenBy(g => g.Id)
					.ToArray(),
					TotalPlayers = g.Games.Sum(g => g.Purchases.Count)
				})
				.OrderByDescending(g => g.TotalPlayers)
				.ThenBy(g => g.Id)
				.ToArray();

			string json = JsonConvert.SerializeObject(genres, Formatting.Indented);
			return json;
		}

		public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
		{
			PurchaseType purchaseType = (PurchaseType)Enum.Parse(typeof(PurchaseType), storeType);
			List<UserExportDto> users = new List<UserExportDto>();

			var usersToProcess = context
				.Purchases
				.AsQueryable()
				.Where(p => p.Type == purchaseType)
				.Include(p => p.Card.User)
				.ToList()
				.GroupBy(p => p.Card.User.Username)
				.ToList();

            foreach (var user in usersToProcess)
            {
				UserExportDto dto = new UserExportDto()
				{
					UserName = user.Key,
					Purchases = user
					.OrderBy(p => p.Date)
					.Select(p => new PurchaseExportDto()
					{
						CardNumber = p.Card.Number,
						Cvc = p.Card.Cvc,
						Date = p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
						Game = new GameDto()
						{
							Tittle = p.Game.Name,
							Genre = p.Game.Genre.Name,
							Price = p.Game.Price,
						}
					})
					.ToList()
				};

				dto.TotalSpent = dto.Purchases.Select(p => p.Game.Price).Sum();
				users.Add(dto);
            }

			users = users
				.Where(u => u.Purchases.Any())
				.OrderByDescending(u => u.TotalSpent)
				.ThenBy(u => u.UserName)
				.ToList();

			XmlSerializer serializer = new XmlSerializer(typeof(UserExportDto[]), new XmlRootAttribute("Users"));
			StringBuilder sb = new StringBuilder();
			XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
			namespaces.Add(string.Empty, string.Empty);

			using (StringWriter writer = new StringWriter(sb))
            {
				serializer.Serialize(writer, users.ToArray(), namespaces);
            }

			return sb.ToString().TrimEnd();
		}
	}
}