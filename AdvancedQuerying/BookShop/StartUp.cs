namespace BookShop
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

            string command = Console.ReadLine().ToLower();

            Console.WriteLine(GetBooksByAgeRestriction(context, command));
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            StringBuilder sb = new StringBuilder();

            var ageRestriction = Enum.Parse<AgeRestriction>(command, true);

            var books = context.Books
                .Where(x => x.AgeRestriction == ageRestriction)
                .OrderBy(x=>x.Title)
                .ToList();

            foreach (var book in books)
            {
                sb
                    .AppendLine(book.Title);
            }
           
            return sb.ToString().TrimEnd();
        }
    }
}
