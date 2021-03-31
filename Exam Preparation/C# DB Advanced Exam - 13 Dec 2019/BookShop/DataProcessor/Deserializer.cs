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
            return "TODO";
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}