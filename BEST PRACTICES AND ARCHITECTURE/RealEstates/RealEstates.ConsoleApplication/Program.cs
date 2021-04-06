namespace RealEstates.ConsoleApplication
{
    using System;
    using Microsoft.EntityFrameworkCore;


    using RealEstates.Data;

    class Program
    {
        static void Main(string[] args)
        {
            var context = new ApplicationDbContext();
            context.Database.Migrate();
        }
    }
}
