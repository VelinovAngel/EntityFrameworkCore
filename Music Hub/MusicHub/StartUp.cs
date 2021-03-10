namespace MusicHub
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);
            //Console.WriteLine(ExportAlbumsInfo(context, 9));
            Console.WriteLine(ExportAlbumsInfo(context, 9));
            //Test your solutions here
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            StringBuilder sb = new StringBuilder();

            var albumsInfo = context.Producers
                .FirstOrDefault(x => x.Id == producerId)
                .Albums
                .Select(album => new
                {
                    albumName = album.Name,
                    releaseDate = album.ReleaseDate,
                    producerName = album.Producer.Name,
                    songs = album.Songs.Select(song => new
                    {
                        songName = song.Name,
                        price = song.Price,
                        writer = song.Writer.Name
                    })
                    .OrderByDescending(x => x.songName)
                    .ThenBy(x => x.writer)
                    .ToList(),
                    totalAlbumPrice = album.Price
                })
                .OrderByDescending(x => x.totalAlbumPrice)
                .ToList();



            foreach (var album in albumsInfo)
            {
                sb.AppendLine($"-AlbumName: {album.albumName}").
                    AppendLine($"-ReleaseDate: {album.releaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)}").
                    AppendLine($"-ProducerName: {album.producerName}").
                    AppendLine($"-Songs:");

                int counter = 1;
                foreach (var songs in album.songs)
                {
                    sb.AppendLine($"---#{counter++}").
                        AppendLine($"---SongName: {songs.songName}")
                        .AppendLine($"---Price: {songs.price:f2}")
                        .AppendLine($"---Writer: {songs.writer}");
                }
                sb.AppendLine($"-AlbumPrice: {album.totalAlbumPrice:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            StringBuilder sb = new StringBuilder();

            //Name, Performer Full Name, Writer Name, Album Producer and Duration 
            var allSongs = context.Songs
                .ToList()
                .Where(x => x.Duration.TotalSeconds > duration)
                .Select(x => new
                {
                    Name = x.Name,
                    Performer = x.SongPerformers.Select(x => x.Performer.FirstName + " " + x.Performer.LastName)
                    .FirstOrDefault(),
                    Writer = x.Writer.Name,
                    AlbumProd = x.Album.Producer.Name,
                    Duration = x.Duration.ToString("c", CultureInfo.InvariantCulture)
                })
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Writer)
                .ThenBy(x => x.Performer)
                .ToList();


            int counter = 1;
            foreach (var song in allSongs)
            {
                //-Song #1
                //---SongName: Away
                //---Writer: Norina Renihan
                //---Performer: Lula Zuan
                //---AlbumProducer: Georgi Milkov
                //---Duration: 00:05:35

                sb
                    .AppendLine($"-Song #{counter++}")
                    .AppendLine($"---SongName: {song.Name}")
                    .AppendLine($"---Writer: {song.Writer}")
                    .AppendLine($"---Performer: {song.Performer}")
                    .AppendLine($"---AlbumProducer: {song.AlbumProd}")
                    .AppendLine($"---Duration: {song.Duration}");
                
            }
            return sb.ToString().TrimEnd();
        }
    }
}
