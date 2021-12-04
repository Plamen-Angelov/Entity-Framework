namespace BookShop.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using BookShop.Common;
    using BookShop.Data.Models;
    using BookShop.Data.Models.Enums;
    using BookShop.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedBook
            = "Successfully imported book {0} for {1:F2}.";

        private const string SuccessfullyImportedAuthor
            = "Successfully imported author - {0} with {1} books.";

        public static string ImportBooks(BookShopContext context, string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(BookImportDto[]), new XmlRootAttribute("Books"));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);

            StringReader reader = new StringReader(xmlString);

            BookImportDto[] dtos = (BookImportDto[])serializer.Deserialize(reader);
            List<Book> books = new List<Book>();
            StringBuilder sb = new StringBuilder();

            foreach (var dto in dtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(GlobalConstants.Error_Message);
                    continue;
                }

                bool isGenreValid = Enum.TryParse(typeof(Genre), dto.Genre.ToString(), out object genre);

                if (!isGenreValid)
                {
                    sb.AppendLine(GlobalConstants.Error_Message);
                    continue;
                }

                bool isDateValid = DateTime.TryParse(dto.PublishedOn, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime publishedOn);

                if (!isDateValid)
                {
                    sb.AppendLine(GlobalConstants.Error_Message);
                    continue;
                }

                Book book = new Book()
                {
                    Name = dto.Name,
                    Genre = (Genre)genre,
                    Price = dto.Price,
                    Pages = dto.Pages,
                    PublishedOn = publishedOn
                };

                books.Add(book);
                sb.AppendLine($"Successfully imported book {book.Name} for {book.Price:F2}.");
            }
            context.AddRange(books);
            context.SaveChanges();

            return sb.ToString().TrimEnd(); ;
        }

        public static string ImportAuthors(BookShopContext context, string jsonString)
        {
            AuthorImportDto[] dtos = JsonConvert.DeserializeObject<AuthorImportDto[]>(jsonString);
            List<Author> authors = new List<Author>();
            StringBuilder sb =  new StringBuilder();

            foreach (var dto in dtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(GlobalConstants.Error_Message);
                    continue;
                }

                if (context.Authors.Any(a => a.Email == dto.Email))
                {
                    sb.AppendLine(GlobalConstants.Error_Message);
                    continue;
                }

                Author author = new Author()
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    Phone = dto.Phone
                };

                List<AuthorBook> books = new List<AuthorBook>();

                foreach (var bookDto in dto.Books)
                {
                    bool isValidId = int.TryParse(bookDto.Id, out int id);

                    if (!isValidId)
                    {
                        continue;
                    }

                    if (!context.Books.Any(b => b.Id == id))
                    {
                        continue;
                    }

                    AuthorBook authorBook = new AuthorBook()
                    {
                        Author = author,
                        BookId = id
                    };

                    books.Add(authorBook);
                }

                if (books.Count == 0)
                {
                    sb.AppendLine(GlobalConstants.Error_Message);
                    continue;
                }

                author.AuthorsBooks = books;
                sb.AppendLine($"Successfully imported author - {author.FirstName} {author.LastName} with {books.Count} books.");
            }
            context.Authors.AddRange(authors);
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