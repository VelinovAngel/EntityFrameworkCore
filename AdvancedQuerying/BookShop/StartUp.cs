namespace BookShop
{
    using Data;
    using Initializer;
    using System;
    using System.Text;

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




            return sb.ToString().TrimEnd();
        }
    }
}
