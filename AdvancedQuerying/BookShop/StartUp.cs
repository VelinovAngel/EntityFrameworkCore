namespace BookShop
{
    using System;
    using System.Globalization;
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

            //int command = int.Parse(Console.ReadLine());

            //Console.WriteLine(GetBooksByAgeRestriction(context, command));
            //Console.WriteLine(GetGoldenBooks(context));
            //Console.WriteLine(GetBooksByPrice(context));
            //Console.WriteLine(GetBooksNotReleasedIn(context, 1998));
            //Console.WriteLine(GetBooksByCategory(context, command));
            //Console.WriteLine(GetBooksReleasedBefore(context, command));
            //Console.WriteLine(GetAuthorNamesEndingIn(context, command));
            //Console.WriteLine(GetBookTitlesContaining(context, command));
            //Console.WriteLine(GetBooksByAuthor(context, command));
            //Console.WriteLine(CountBooks(context, command));
            //Console.WriteLine(CountCopiesByAuthor(context));
            //Console.WriteLine(GetTotalProfitByCategory(context));
            Console.WriteLine(GetMostRecentBooks(context));
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
                .Select(x => x.ToLower())
                .ToArray();

            var books = context.Books
                .Select(x => new
                {
                    x.Title,
                    x.BookCategories

                })
                .Where(x => x.BookCategories.Any(x => command.Contains(x.Category.Name.ToLower())))
                .OrderBy(x => x.Title)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb
                    .AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {

            var books = context.Books
                .Select(x => new
                {
                    title = x.Title,
                    type = x.EditionType,
                    price = x.Price,
                    releaseDate = x.ReleaseDate
                })
                .Where(x => x.releaseDate.Value < DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture))
                .OrderByDescending(x => x.releaseDate)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb
                    .AppendLine($"{book.title} - {book.type} - ${book.price:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors
                .Where(x => x.FirstName.EndsWith(input))
                .Select(x => new
                {
                    fullName = x.FirstName + " " + x.LastName
                })
                .OrderBy(x => x.fullName)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var author in authors)
            {

                sb
                    .AppendLine(author.fullName);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books
                .Select(x => new
                {
                    title = x.Title
                })
                .Where(x => x.title.ToLower().Contains(input.ToLower()))
                .OrderBy(x => x.title)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb
                    .AppendLine(book.title);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var booksAndAuthors = context.Books
                .Select(x => new
                {
                    bookId = x.BookId,
                    title = x.Title,
                    firstName = x.Author.FirstName,
                    lastName = x.Author.LastName
                })
                .Where(x => x.lastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(x => x.bookId)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in booksAndAuthors)
            {
                sb
                    .AppendLine($"{book.title} ({book.firstName} {book.lastName})");
            }

            return sb.ToString().TrimEnd();
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var books = context.Books
                .Select(x => new
                {
                    title = x.Title
                })
                .Where(x => x.title.Length > lengthCheck)
                .ToList();

            return books.Count();
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var books = context.Authors
                .Select(x => new
                {
                    numberBooks = x.Books.Sum(x => x.Copies),
                    firstName = x.FirstName,
                    lastName = x.LastName
                })
                .OrderByDescending(x => x.numberBooks)
                .ToList();

            StringBuilder sb = new StringBuilder();

            var result = string.Join(Environment.NewLine, books
                .Select(x => $"{x.firstName} {x.lastName} - {x.numberBooks}"));

            return result;
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categories = context.Categories
                .Select(x => new
                {
                    name = x.Name,
                    profit = x.CategoryBooks.Sum(x => x.Book.Price * x.Book.Copies)

                })
                .OrderByDescending(x => x.profit)
                .ThenBy(c => c.name)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var category in categories)
            {
                sb
                    .AppendLine($"{category.name} ${category.profit:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var mostRecentBooks = context.Categories
                .Select(x => new
                {
                    categoryName = x.Name,
                    bookName = x.CategoryBooks.Select(x => new { book = x.Book.Title, releaseData = x.Book.ReleaseDate })
                    .OrderByDescending(x => x.releaseData)
                    .Take(3)
                    .ToList()
                })
                .OrderBy(x => x.categoryName)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var category in mostRecentBooks)
            {
                sb
                    .AppendLine($"--{category.categoryName}");
                foreach (var book in category.bookName)
                {
                    sb.AppendLine($"{book.book} ({book.releaseData.Value.Year})");
                }
            }

            return sb.ToString().TrimEnd();
        }
    }


}

