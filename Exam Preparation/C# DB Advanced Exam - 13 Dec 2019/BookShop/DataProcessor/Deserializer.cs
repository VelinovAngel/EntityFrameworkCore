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
    using BookShop.Data.Models;
    using BookShop.Data.Models.Enums;
    using BookShop.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;


    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedBook
            = "Successfully imported book {0} for {1:F2}.";

        private const string SuccessfullyImportedAuthor
            = "Successfully imported author - {0} with {1} books.";

        public static string ImportBooks(BookShopContext context, string xmlString)
        {
            var result = new StringBuilder();

            var xmlSerializer = new XmlSerializer(typeof(ImportBooksModel[]), new XmlRootAttribute("Books"));

            var stringReader = new StringReader(xmlString);

            var booksDto = (IEnumerable<ImportBooksModel>)xmlSerializer.Deserialize(stringReader);

            foreach (var currBook in booksDto)
            {
                if (!IsValid(currBook))
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                var book = new Book
                {
                    Name = currBook.Name,
                    Genre = (Genre)currBook.Genre,
                    Pages = currBook.Pages,
                    Price = currBook.Price,
                    PublishedOn = DateTime.ParseExact(currBook.PublishedOn.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture)
                };

                context.Books.Add(book);
                context.SaveChanges();
                result.AppendLine(string.Format(SuccessfullyImportedBook, currBook.Name, currBook.Price));
            }

            return result.ToString().TrimEnd();
        }

        public static string ImportAuthors(BookShopContext context, string jsonString)
        {
            var result = new StringBuilder();

            var authorsJson = JsonConvert.DeserializeObject<IEnumerable<ImportAuthorsModel>>(jsonString);

            List<Author> authors = new List<Author>();

            foreach (var authorDto in authorsJson)
            {
                if (!IsValid(authorDto))
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                bool doesEmailExists = authors
                    .FirstOrDefault(x => x.Email == authorDto.Email) != null;

                if (doesEmailExists)
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                var author = new Author
                {
                    FirstName = authorDto.FirstName,
                    LastName = authorDto.LastName,
                    Email = authorDto.Email,
                    Phone = authorDto.Phone
                };

                var uniqueBookIds = authorDto.Books.Distinct();

                foreach (var authorDtoAuthorBookDto in uniqueBookIds)
                {
                    var book = context.Books.Find(authorDtoAuthorBookDto.Id);

                    if (book == null)
                    {
                        continue;
                    }

                    author.AuthorsBooks.Add(new AuthorBook
                    {
                        Author = author,
                        Book = book
                    });
                }

                if (author.AuthorsBooks.Count == 0)
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                authors.Add(author);
                result.AppendLine(string.Format(SuccessfullyImportedAuthor, (author.FirstName + " " + author.LastName), author.AuthorsBooks.Count));
            }

            context.Authors.AddRange(authors);
            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}