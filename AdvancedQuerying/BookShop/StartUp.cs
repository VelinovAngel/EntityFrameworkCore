﻿namespace BookShop
{
    using System;
    using System.Linq;
    using System.Text;
    using BookShop.Models.Enums;
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main()
        {
            using var context = new BookShopContext();
            DbInitializer.ResetDatabase(context);

            string command = Console.ReadLine();

            //Console.WriteLine(GetBooksByAgeRestriction(context, command));
            //Console.WriteLine(GetGoldenBooks(context));
            //Console.WriteLine(GetBooksByPrice(context));
            //Console.WriteLine(GetBooksNotReleasedIn(context, 1998));
            Console.WriteLine(GetBooksByCategory(context, command));
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            StringBuilder sb = new StringBuilder();

            var ageRestriction = Enum.Parse<AgeRestriction>(command, true);

            var books = context.Books
                .Where(x => x.AgeRestriction == ageRestriction)
                .OrderBy(x => x.Title)
                .ToList();

            foreach (var book in books)
            {
                sb
                    .AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Books
                .Where(x => x.EditionType == EditionType.Gold && x.Copies < 5000)
                .OrderBy(x => x.BookId)
                .ToList();

            foreach (var book in books)
            {
                sb
                    .AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Books
                .Where(x => x.Price > 40)
                .Select(x => new
                {
                    title = x.Title,
                    price = x.Price
                })
                .OrderByDescending(x => x.price)
                .ToList();

            foreach (var book in books)
            {
                sb
                    .AppendLine($"{book.title} - ${book.price:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Books
                .Where(x => x.ReleaseDate.Value.Year != year)
                .OrderBy(x => x.BookId)
                .ToList();

            foreach (var book in books)
            {
                sb
                    .AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var command = input
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(x=>x.ToLower())
                .ToArray();

            var books = context.Books
                .Select(x => new
                {
                    x.Title,
                    x.BookCategories

                })
                .Where(x => x.BookCategories.Any(x => command.Contains(x.Category.Name.ToLower())))
                .OrderBy(x=>x.Title)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb
                    .AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();
        }
    }
}
