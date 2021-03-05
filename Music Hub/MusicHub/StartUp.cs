namespace MusicHub
{
    using System;
    using System.Linq;
    using System.Text;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            MusicHubDbContext context = 
                new MusicHubDbContext();

            //DbInitializer.ResetDatabase(context);
            Console.WriteLine(ExportAlbumsInfo(context, 9));
            //Test your solutions here
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            StringBuilder sb = new StringBuilder();

            var albums = context.Albums
                .Where(x => x.ProducerId == producerId)
                .ToList();
                

            foreach (var album in albums)
            {
                sb.AppendLine($"{album.Name}").
                    AppendLine($"{album.ReleaseDate}");
                foreach (var songs in album.Songs)
                {
                    sb.AppendLine($"{songs.Name}")
                        .AppendLine($"{songs.Price:f2}")
                        .AppendLine($"{songs.Writer}");
                }
            }   

            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            throw new NotImplementedException();
        }
    }
}
