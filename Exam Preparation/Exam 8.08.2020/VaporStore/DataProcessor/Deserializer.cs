namespace VaporStore.DataProcessor
{
	using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Common;
    using VaporStore.Data.Models;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.Dto.Import;

    public static class Deserializer
	{
		public static string ImportGames(VaporStoreDbContext context, string jsonString)
		{
			GameImportDto[] gameDtos = JsonConvert.DeserializeObject<GameImportDto[]>(jsonString);
			StringBuilder sb = new StringBuilder();
			List<Game> games = new List<Game>();
			List<Developer> devs = new List<Developer>();
			List<Genre> genres = new List<Genre>();
			List<Tag> tags = new List<Tag>();

			foreach (GameImportDto dto in gameDtos)
            {
                if (!IsValid(dto))
                {
					sb.AppendLine(GlobalConstants.Error_Message);
					continue;
                }

                DateTime releaseDate;
				bool isReleaseDateValid = DateTime.TryParse(dto.ReleaseDate, CultureInfo.InvariantCulture, 
					DateTimeStyles.None, out releaseDate);

				if (!isReleaseDateValid)
                {
					sb.AppendLine(GlobalConstants.Error_Message);
					continue;
				}

				Developer developer = devs
					.FirstOrDefault(d => d.Name == dto.Developer);

				if (developer == null)
                {
					developer = new Developer()
                    {
						Name = dto.Developer
                    };

					devs.Add(developer);
                }

				Genre genre = genres
					.FirstOrDefault(g => g.Name == dto.Genre);

                if (genre == null)
                {
					genre = new Genre()
					{
						Name = dto.Genre
					};

					genres.Add(genre);
                }

				Game game = new Game()
                {
					Name = dto.Name,
					Price = dto.Price,
					ReleaseDate = releaseDate,
					Developer = developer,
					Genre = genre
                };

				HashSet<GameTag> gameTags = new HashSet<GameTag>();

                foreach (var t in dto.Tags)
                {
					Tag tag = tags
						.FirstOrDefault(tag => tag.Name == t);

                    if (tag == null)
                    {
						tag = new Tag()
						{
							Name = t
						};

						tags.Add(tag);
                    }

					GameTag gameTag = new GameTag()
					{
						Tag = tag,
						Game = game
					};

					gameTags.Add(gameTag);
                }

				game.GameTags = gameTags;
				games.Add(game);
				sb.AppendLine($"Added { game.Name} ({ genre.Name}) with { gameTags.Count} tags");
            }

			context.Games.AddRange(games);
			//context.Developers.AddRange(devs);
			//context.Genres.AddRange(genres);
			//context.Tags.AddRange(tags);
			context.SaveChanges();

			return sb.ToString().TrimEnd();
		}

		public static string ImportUsers(VaporStoreDbContext context, string jsonString)
		{
			UserImportDto[] userDtos = JsonConvert.DeserializeObject<UserImportDto[]>(jsonString);
			StringBuilder sb = new StringBuilder();
			List<User> users = new List<User>();

            foreach (var userDto in userDtos)
            {
                if (!IsValid(userDto))
                {
					sb.AppendLine(GlobalConstants.Error_Message);
					continue;
                }

				User user = new User()
				{
					Username = userDto.Username,
					FullName = userDto.FullName,
					Email = userDto.Email,
					Age = userDto.Age
				};

				List<Card> cards = new List<Card>();
				bool hasInvalidCard = false;

                foreach (var cardDto in userDto.Cards)
                {
                    if (!IsValid(cardDto))
                    {
						sb.AppendLine(GlobalConstants.Error_Message);
						hasInvalidCard = true;
						break;
					}

					Card card = new Card()
					{
						Number = cardDto.Number,
						Cvc = cardDto.Cvc
					};

					CardType type;
					bool isCardTypeValid = Enum.TryParse<CardType>(cardDto.Type, true, out type);

					if (!isCardTypeValid)
                    {
						sb.AppendLine(GlobalConstants.Error_Message);
						hasInvalidCard = true;
						break;
					}
					card.Type = type;
					cards.Add(card);
                }

                if (hasInvalidCard)
                {
					sb.AppendLine(GlobalConstants.Error_Message);
					continue;
				}

				user.Cards = cards;
				users.Add(user);
				sb.AppendLine($"Imported {user.Username} with {user.Cards.Count} cards");
            }

			context.Users.AddRange(users);
			context.SaveChanges();

			return sb.ToString().TrimEnd();
		}

		public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(PurchaseImportDto[]), new XmlRootAttribute("Purchases"));
			PurchaseImportDto[] purchaseDtos;


			using (StringReader reader = new StringReader(xmlString))
            {
				purchaseDtos = (PurchaseImportDto[])serializer.Deserialize(reader);
            }

			StringBuilder sb = new StringBuilder();
			List<Purchase> purchases = new List<Purchase>();
			List<Game> games = context
				.Games
				.ToList();
			List<Card> cards = context
				.Cards
				.ToList();

            foreach (var dto in purchaseDtos)
            {
                if (!IsValid(dto))
                {
					sb.AppendLine(GlobalConstants.Error_Message);
					continue;
                }

				bool isPurchaseTypeValid = Enum.TryParse(typeof(PurchaseType), dto.Type, out object purchaseType);

				if (!isPurchaseTypeValid)
                {
					sb.AppendLine(GlobalConstants.Error_Message);
					continue;
				}

				bool isDateValid = DateTime.TryParseExact(dto.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, 
					DateTimeStyles.None, out DateTime date);

                if (!isDateValid)
                {
					sb.AppendLine(GlobalConstants.Error_Message);
					continue;
				}

				Card card = cards
					.FirstOrDefault(c => c.Number == dto.CardNumber);

				if (card == null)
                {
					sb.AppendLine(GlobalConstants.Error_Message);
					continue;
                }

				Game game = games
					.FirstOrDefault(g => g.Name == dto.GameName);

				if (game == null)
                {
					sb.AppendLine(GlobalConstants.Error_Message);
					continue;
                }

                Purchase purchase = new Purchase()
				{
					Type = (PurchaseType)purchaseType,
					ProductKey = dto.ProductKey,
					Date = date,
					Card = card,
					Game = game
				};

				purchases.Add(purchase);
				sb.AppendLine($"Imported {dto.GameName} for {card.User.Username}");
            }

			context.Purchases.AddRange(purchases);
			context.SaveChanges();
			return sb.ToString().TrimEnd();
		}

		private static bool IsValid(object dto)
		{
			var validationContext = new ValidationContext(dto);
			var validationResult = new List<ValidationResult>();

			return Validator.TryValidateObject(dto, validationContext, validationResult, true);
		}
	}
}